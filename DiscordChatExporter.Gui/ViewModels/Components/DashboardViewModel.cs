﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DiscordChatExporter.Core.Discord;
using DiscordChatExporter.Core.Discord.Data;
using DiscordChatExporter.Core.Exceptions;
using DiscordChatExporter.Core.Exporting;
using DiscordChatExporter.Core.Utils.Extensions;
using DiscordChatExporter.Gui.Services;
using DiscordChatExporter.Gui.Utils;
using DiscordChatExporter.Gui.ViewModels.Dialogs;
using DiscordChatExporter.Gui.ViewModels.Messages;
using DiscordChatExporter.Gui.ViewModels.Framework;
using Gress;
using Gress.Completable;
using Stylet;

namespace DiscordChatExporter.Gui.ViewModels.Components;

public class DashboardViewModel : PropertyChangedBase
{
    private readonly IViewModelFactory _viewModelFactory;
    private readonly IEventAggregator _eventAggregator;
    private readonly DialogManager _dialogManager;
    private readonly SettingsService _settingsService;

    private readonly AutoResetProgressMuxer _progressMuxer;

    private DiscordClient? _discord;

    public bool IsBusy { get; private set; }

    public ProgressContainer<Percentage> Progress { get; } = new();

    public bool IsProgressIndeterminate => IsBusy && Progress.Current.Fraction is <= 0 or >= 1;

    public string? Token { get; set; }

    private IReadOnlyDictionary<Guild, IReadOnlyList<Channel>>? ChannelsByGuild { get; set; }

    public IReadOnlyList<Guild>? AvailableGuilds => ChannelsByGuild?.Keys.ToArray();

    public Guild? SelectedGuild { get; set; }

    public bool IsDirectMessageGuildSelected => SelectedGuild?.Id == Guild.DirectMessages.Id;

    public IReadOnlyList<Channel>? AvailableChannels => SelectedGuild is not null
        ? ChannelsByGuild?[SelectedGuild]
        : null;

    public IReadOnlyList<Channel>? SelectedChannels { get; set; }

    public DashboardViewModel(
        IViewModelFactory viewModelFactory,
        IEventAggregator eventAggregator,
        DialogManager dialogManager,
        SettingsService settingsService)
    {
        _viewModelFactory = viewModelFactory;
        _eventAggregator = eventAggregator;
        _dialogManager = dialogManager;
        _settingsService = settingsService;

        _progressMuxer = Progress.CreateMuxer().WithAutoReset();

        this.Bind(
            o => o.IsBusy,
            (_, _) => NotifyOfPropertyChange(() => IsProgressIndeterminate)
        );

        Progress.Bind(
            o => o.Current,
            (_, _) => NotifyOfPropertyChange(() => IsProgressIndeterminate)
        );
    }

    public void OnViewLoaded()
    {
        if (!string.IsNullOrWhiteSpace(_settingsService.LastToken))
            Token = _settingsService.LastToken;
    }

    public async void ShowSettings()
    {
        var dialog = _viewModelFactory.CreateSettingsViewModel();
        await _dialogManager.ShowDialogAsync(dialog);
    }

    public void ShowHelp() => ProcessEx.StartShellExecute(App.DocumentationUrl);

    public bool CanPopulate => !IsBusy && !string.IsNullOrWhiteSpace(Token);

    public async void Populate()
    {
        IsBusy = true;
        var progress = _progressMuxer.CreateInput();

        try
        {
            var token = Token?.Trim('"', ' ');
            if (string.IsNullOrWhiteSpace(token))
                return;

            _settingsService.LastToken = token;

            var discord = new DiscordClient(token);

            var channelsByGuild = new Dictionary<Guild, IReadOnlyList<Channel>>();
            await foreach (var guild in discord.GetUserGuildsAsync())
            {
                channelsByGuild[guild] = (await discord.GetGuildChannelsAsync(guild.Id))
                    .Where(c => c.Kind != ChannelKind.GuildCategory)
                    .ToArray();
            }

            _discord = discord;
            ChannelsByGuild = channelsByGuild;
            SelectedGuild = channelsByGuild.Keys.FirstOrDefault();
        }
        catch (DiscordChatExporterException ex) when (!ex.IsFatal)
        {
            _eventAggregator.Publish(
                new NotificationMessage(ex.Message.TrimEnd('.'))
            );
        }
        catch (Exception ex)
        {
            var dialog = _viewModelFactory.CreateMessageBoxViewModel(
                "Error pulling guilds and channels",
                ex.ToString()
            );

            await _dialogManager.ShowDialogAsync(dialog);
        }
        finally
        {
            progress.ReportCompletion();
            IsBusy = false;
        }
    }

    public bool CanExport =>
        !IsBusy &&
        _discord is not null &&
        SelectedGuild is not null &&
        SelectedChannels?.Any() is true;

    public async void Export()
    {
        IsBusy = true;

        try
        {
            if (_discord is null || SelectedGuild is null || SelectedChannels is null || !SelectedChannels.Any())
                return;

            var dialog = _viewModelFactory.CreateExportSetupViewModel(SelectedGuild, SelectedChannels);
            if (await _dialogManager.ShowDialogAsync(dialog) != true)
                return;

            var exporter = new ChannelExporter(_discord);

            var channelProgressPairs = dialog.Channels!.Select(c => new
            {
                Channel = c,
                Progress = _progressMuxer.CreateInput()
            }).ToArray();

            var successfulExportCount = 0;

            await Parallel.ForEachAsync(
                channelProgressPairs,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = Math.Max(1, _settingsService.ParallelLimit)
                },
                async (pair, cancellationToken) =>
                {
                    var channel = pair.Channel;
                    var progress = pair.Progress;

                    try
                    {
                        var request = new ExportRequest(
                            dialog.Guild!,
                            channel,
                            dialog.OutputPath!,
                            dialog.AssetsDirPath,
                            dialog.SelectedFormat,
                            dialog.After?.Pipe(Snowflake.FromDate),
                            dialog.Before?.Pipe(Snowflake.FromDate),
                            dialog.PartitionLimit,
                            dialog.MessageFilter,
                            dialog.ShouldFormatMarkdown,
                            dialog.ShouldDownloadAssets,
                            dialog.ShouldReuseAssets,
                            _settingsService.DateFormat
                        );

                        await exporter.ExportChannelAsync(request, progress, cancellationToken);

                        Interlocked.Increment(ref successfulExportCount);
                    }
                    catch (DiscordChatExporterException ex) when (!ex.IsFatal)
                    {
                        _eventAggregator.Publish(
                            new NotificationMessage(ex.Message.TrimEnd('.') + $" ({channel.Name})")
                        );
                    }
                    finally
                    {
                        progress.ReportCompletion();
                    }
                }
            );

            // Notify of the overall completion
            if (successfulExportCount > 0)
            {
                _eventAggregator.Publish(
                    new NotificationMessage($"Successfully exported {successfulExportCount} channel(s)")
                );
            }
        }
        catch (Exception ex)
        {
            var dialog = _viewModelFactory.CreateMessageBoxViewModel(
                "Error exporting channel(s)",
                ex.ToString()
            );

            await _dialogManager.ShowDialogAsync(dialog);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void OpenDiscord() =>
        ProcessEx.StartShellExecute("https://discord.com/app");

    public void OpenDiscordDeveloperPortal() =>
        ProcessEx.StartShellExecute("https://discord.com/developers/applications");
}
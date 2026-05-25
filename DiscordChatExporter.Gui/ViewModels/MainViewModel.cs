using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia;
using DiscordChatExporter.Gui.Framework;
using DiscordChatExporter.Gui.Localization;
using DiscordChatExporter.Gui.Services;
using DiscordChatExporter.Gui.Utils.Extensions;
using DiscordChatExporter.Gui.ViewModels.Components;
using PowerKit.Extensions;

namespace DiscordChatExporter.Gui.ViewModels;

public partial class MainViewModel(
    ViewModelManager viewModelManager,
    SnackbarManager snackbarManager,
    SettingsService settingsService,
    UpdateService updateService,
    LocalizationManager localizationManager
) : ViewModelBase
{
    public string Title { get; } = $"{Program.Name} v{Program.VersionString}";

    public DashboardViewModel Dashboard { get; } = viewModelManager.GetDashboardViewModel();

    private async Task CheckForUpdatesAsync()
    {
        try
        {
            var updateVersion = await updateService.CheckForUpdatesAsync();
            if (updateVersion is null)
                return;

            snackbarManager.Notify(
                string.Format(
                    localizationManager.UpdateDownloadingMessage,
                    Program.Name,
                    updateVersion
                )
            );
            await updateService.PrepareUpdateAsync(updateVersion);

            snackbarManager.Notify(
                localizationManager.UpdateReadyMessage,
                localizationManager.UpdateInstallNowButton,
                () =>
                {
                    updateService.FinalizeUpdate(true);

                    if (Application.Current?.ApplicationLifetime?.TryShutdown(2) != true)
                        Environment.Exit(2);
                }
            );
        }
        catch
        {
            // Failure to update shouldn't crash the application
            snackbarManager.Notify(localizationManager.UpdateFailedMessage);
        }
    }

    public override async Task InitializeAsync()
    {
        await CheckForUpdatesAsync();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Save settings
            settingsService.Save();

            // Finalize pending updates
            updateService.FinalizeUpdate(false);
        }

        base.Dispose(disposing);
    }
}

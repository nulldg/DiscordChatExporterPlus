﻿using System;
using DiscordChatExporter.Core.Exporting;
using Microsoft.Win32;
using Tyrrrz.Settings;

namespace DiscordChatExporter.Gui.Services;

public partial class SettingsService : SettingsManager
{
    public bool IsAutoUpdateEnabled { get; set; } = true;

    public bool IsDarkModeEnabled { get; set; } = IsDarkModeEnabledByDefault();

    public bool IsTokenPersisted { get; set; } = true;

    public string DateFormat { get; set; } = "MM/dd/yyyy h:mm tt";

    public int ParallelLimit { get; set; } = 1;

    public Version? LastAppVersion { get; set; }

    public string? LastToken { get; set; }

    public ExportFormat LastExportFormat { get; set; } = ExportFormat.HtmlDark;

    public string? LastPartitionLimitValue { get; set; }

    public string? LastMessageFilterValue { get; set; }

    public bool LastShouldFormatMarkdown { get; set; } = true;

    public bool LastShouldDownloadAssets { get; set; }

    public bool LastShouldReuseAssets { get; set; }

    public string? LastAssetsDirPath { get; set; }

    public SettingsService()
    {
        Configuration.StorageSpace = StorageSpace.Instance;
        Configuration.SubDirectoryPath = "";
        Configuration.FileName = "Settings.dat";
    }

    public bool ShouldSerializeLastToken() => IsTokenPersisted;
}

public partial class SettingsService
{
    private static bool IsDarkModeEnabledByDefault()
    {
        try
        {
            return Registry.CurrentUser.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
                false
            )?.GetValue("AppsUseLightTheme") is 0;
        }
        catch
        {
            return false;
        }
    }
}
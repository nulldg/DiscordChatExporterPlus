# DiscordChatExporterPlus

This is a fork of **DiscordChatExporter** with the political cancer, popups & notices, protestware "sanctions", general discrimination against Russian people, etc removed.

This fork acts as a drop-in replacement for DiscordChatExporter, and is set to update from this fork instead of upstream. Upstream is treated with minimal trust, therefore commits from upstream and the dependencies owned by Tyrrrz are reviewed to avoid supply-chain attacks and insertion of political bloat.

----

**DiscordChatExporter** is an application that can be used to export message history from any [Discord](https://discord.com) channel to a file.
It works with direct messages, group messages, and server channels, and supports Discord's dialect of markdown as well as most other rich media features.

> ❔ If you have questions or issues, **please refer to the [docs](.docs)**.

This application comes in two flavors: graphical user interface (**GUI**) and command-line interface (**CLI**).
Supported operating systems are Windows 7 or higher, macOS 10.13 (High Sierra) or higher, and Linux.

## Installation

To install this fork, download the [latest release](https://github.com/nulldg/DiscordChatExporterPlus/releases/latest) and extract the zip into an empty directory.

If you already have a copy of DiscordChatExporter and wish to switch to this fork, you can simply overwrite the existing files. Your settings will be preserved.

> [!IMPORTANT]
> To launch the GUI version of the app on MacOS, you need to first remove the downloaded file from quarantine.
> You can do that by running the following command in the terminal: `xattr -rd com.apple.quarantine DiscordChatExporter.app`.

> [!NOTE]
> If you're unsure which build is right for your system, consult with [this page](https://useragent.cc) to determine your OS and CPU architecture.

## Features

- Cross-platform graphical and command-line interfaces
- Authentication via either a user or a bot token
- Multiple output formats: HTML (dark/light), TXT, CSV, JSON
- Support for markdown, attachments, embeds, emoji, and other rich media features
- File partitioning, date ranges, message filtering, and other export options
- Self-contained exports which can be viewed offline

## Screenshots

![channel list](.assets/list.png)
![rendered output](.assets/output.png)

## See also

- [**Chat Analytics**](https://github.com/mlomb/chat-analytics) — solution for analyzing chat patterns of Discord users, using exports produced by **DiscordChatExporter**.
- [**DiscordChatExporter-frontend**](https://github.com/slatinsky/DiscordChatExporter-frontend) — convenient viewer for exports produced by **DiscordChatExporter**.

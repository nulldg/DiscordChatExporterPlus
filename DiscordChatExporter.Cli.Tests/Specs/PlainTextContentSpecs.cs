﻿using System.Threading.Tasks;
using DiscordChatExporter.Cli.Tests.Fixtures;
using DiscordChatExporter.Cli.Tests.TestData;
using FluentAssertions;
using Xunit;

namespace DiscordChatExporter.Cli.Tests.Specs;

[Collection(nameof(ExportWrapperCollection))]
public class PlainTextContentSpecs
{
    private readonly ExportWrapperFixture _exportWrapper;

    public PlainTextContentSpecs(ExportWrapperFixture exportWrapper)
    {
        _exportWrapper = exportWrapper;
    }

    [Fact]
    public async Task Messages_are_exported_correctly()
    {
        // Act
        var document = await _exportWrapper.ExportAsPlainTextAsync(ChannelIds.DateRangeTestCases);

        // Assert
        document.Should().ContainAll(
            "Tyrrrz#5447",
            "Hello world",
            "Goodbye world",
            "Foo bar",
            "Hurdle Durdle",
            "One",
            "Two",
            "Three",
            "Yeet"
        );
    }
}
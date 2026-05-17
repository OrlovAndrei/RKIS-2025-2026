using System;
using System.Collections.Generic;
using System.Linq;
using TodoList.commands;
using TodoList.Exceptions;
using Xunit;

namespace TodoList.Tests;

public class CommandParserTests
{
    [Theory]
    [InlineData("help", typeof(HelpCommand))]
    [InlineData("add task", typeof(AddCommand))]
    [InlineData("view", typeof(ViewCommand))]
    [InlineData("read 1", typeof(ReadCommand))]
    [InlineData("setstatus 1 InProgress", typeof(SetStatusCommand))]
    [InlineData("delete 1", typeof(DeleteCommand))]
    [InlineData("update 1 new text", typeof(UpdateCommand))]
    [InlineData("profile", typeof(ProfileCommand))]
    [InlineData("undo", typeof(UndoCommand))]
    [InlineData("redo", typeof(RedoCommand))]
    [InlineData("exit", typeof(ExitCommand))]
    [InlineData("search --contains text", typeof(SearchCommand))]
    [InlineData("load 3 100", typeof(LoadCommand))]
    public void Parse_ValidCommand_ReturnsCorrectCommandType(string input, Type expectedType)
    {
        var command = CommandParser.Parse(input);
        Assert.IsType(expectedType, command);
    }

    [Fact]
    public void Parse_UnknownCommand_ThrowsInvalidCommandException()
    {
        Assert.Throws<InvalidCommandException>(() => CommandParser.Parse("unknown"));
    }

    [Fact]
    public void ParseAdd_WithSimpleText_SetsPartsCorrectly()
    {
        var command = CommandParser.Parse("add Купить молоко") as AddCommand;
        Assert.NotNull(command);
        Assert.False(command.multiline);
        Assert.Equal(new[] { "add", "Купить", "молоко" }, command.parts);
    }

    [Fact]
    public void ParseAdd_WithMultilineFlag_SetsMultilineTrue()
    {
        var command = CommandParser.Parse("add -m") as AddCommand;
        Assert.NotNull(command);
        Assert.True(command.multiline);
        Assert.Equal(new[] { "add" }, command.parts);
    }

    [Fact]
    public void ParseView_WithFlags_SetsProperties()
    {
        var command = CommandParser.Parse("view -isd") as ViewCommand;
        Assert.NotNull(command);
        Assert.True(command.ShowIndex);
        Assert.True(command.ShowStatus);
        Assert.True(command.ShowDate);
        Assert.False(command.ShowAll);
    }

    [Fact]
    public void ParseView_WithAllFlag_SetsShowAll()
    {
        var command = CommandParser.Parse("view --all") as ViewCommand;
        Assert.NotNull(command);
        Assert.True(command.ShowAll);
        Assert.True(command.ShowIndex);
        Assert.True(command.ShowStatus);
        Assert.True(command.ShowDate);
    }

    [Fact]
    public void ParseRead_WithNumber_SetsParts()
    {
        var command = CommandParser.Parse("read 5") as ReadCommand;
        Assert.NotNull(command);
        Assert.Equal(new[] { "read", "5" }, command.parts);
    }

    [Fact]
    public void ParseSetStatus_WithNumberAndStatus_SetsParts()
    {
        var command = CommandParser.Parse("setstatus 2 Completed") as SetStatusCommand;
        Assert.NotNull(command);
        Assert.Equal(new[] { "setstatus", "2", "Completed" }, command.parts);
    }

    [Fact]
    public void ParseDelete_WithNumber_SetsParts()
    {
        var command = CommandParser.Parse("delete 3") as DeleteCommand;
        Assert.NotNull(command);
        Assert.Equal(new[] { "delete", "3" }, command.parts);
    }

    [Fact]
    public void ParseUpdate_WithNumberAndText_SetsParts()
    {
        var command = CommandParser.Parse("update 1 Новый текст") as UpdateCommand;
        Assert.NotNull(command);
        Assert.Equal(new[] { "update", "1", "Новый", "текст" }, command.parts);
    }

    [Fact]
    public void ParseProfile_WithoutArgs_SetsPartsOnlyCommand()
    {
        var command = CommandParser.Parse("profile") as ProfileCommand;
        Assert.NotNull(command);
        Assert.Equal(new[] { "profile" }, command.parts);
    }

    [Fact]
    public void ParseProfile_WithOutFlag_SetsPartsWithFlag()
    {
        var command = CommandParser.Parse("profile --out") as ProfileCommand;
        Assert.NotNull(command);
        Assert.Equal(new[] { "profile", "--out" }, command.parts);
    }

    [Fact]
    public void ParseSearch_WithContainsFlag_SetsContainsText()
    {
        var command = CommandParser.Parse("search --contains \"work at\"") as SearchCommand;
        Assert.NotNull(command);
        Assert.Equal("work at", command.ContainsText);
    }

    [Fact]
    public void ParseSearch_WithMultipleFlags_SetsProperties()
    {
        var command = CommandParser.Parse("search --status InProgress --sort date --desc --top 5") as SearchCommand;
        Assert.NotNull(command);
        Assert.Equal(TodoStatus.InProgress, command.Status);
        Assert.Equal("date", command.SortBy);
        Assert.True(command.SortDescending);
        Assert.Equal(5, command.Top);
    }

    [Fact]
    public void ParseSearch_WithInvalidDate_ThrowsInvalidArgumentException()
    {
        Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse("search --from invalid-date"));
    }

    [Fact]
    public void ParseSearch_WithUnknownFlag_ThrowsInvalidArgumentException()
    {
        Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse("search --unknown flag"));
    }

    [Fact]
    public void ParseLoad_WithTwoNumbers_SetsParts()
    {
        var command = CommandParser.Parse("load 5 100") as LoadCommand;
        Assert.NotNull(command);
        Assert.Equal(new[] { "load", "5", "100" }, command.parts);
    }

    [Fact]
    public void ParseLoad_WithInvalidArguments_ThrowsInvalidArgumentException()
    {
        Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse("load 0 100"));
    }
}
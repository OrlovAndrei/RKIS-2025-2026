using System;
using System.Collections.Generic;
using Xunit;
using Todolist.Exceptions;

namespace Todolist.Tests
{
    public class CommandParserTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Parse_WithEmptyInput_ReturnsNull(string input)
        {
            var result = CommandParser.Parse(input);
            Assert.Null(result);
        }

        [Fact]
        public void Parse_HelpCommand_ReturnsHelpCommand()
        {
            var result = CommandParser.Parse("help");
            Assert.IsType<HelpCommand>(result);
        }

        [Fact]
        public void Parse_ExitCommand_ReturnsExitCommand()
        {
            var result = CommandParser.Parse("exit");
            Assert.IsType<ExitCommand>(result);
        }

        [Fact]
        public void Parse_UndoCommand_ReturnsUndoCommand()
        {
            var result = CommandParser.Parse("undo");
            Assert.IsType<UndoCommand>(result);
        }

        [Fact]
        public void Parse_RedoCommand_ReturnsRedoCommand()
        {
            var result = CommandParser.Parse("redo");
            Assert.IsType<RedoCommand>(result);
        }

        [Theory]
        [InlineData("profile")]
        [InlineData("profile -o")]
        [InlineData("profile --out")]
        public void Parse_ProfileCommand_ReturnsProfileCommand(string input)
        {
            var result = CommandParser.Parse(input);
            Assert.IsType<ProfileCommand>(result);
        }

        [Theory]
        [InlineData("view", false, false, false, false)]
        [InlineData("view -i", true, false, false, false)]
        [InlineData("view --index", true, false, false, false)]
        [InlineData("view -s", false, true, false, false)]
        [InlineData("view --status", false, true, false, false)]
        [InlineData("view -d", false, false, true, false)]
        [InlineData("view --update-date", false, false, true, false)]
        [InlineData("view -a", false, false, false, true)]
        [InlineData("view --all", false, false, false, true)]
        [InlineData("view -i -s -d", true, true, true, false)]
        public void Parse_ViewCommand_WithFlags_ReturnsCorrectViewCommand(
            string input, bool showIndex, bool showStatus, bool showDate, bool showAll)
        {
            var result = CommandParser.Parse(input) as ViewCommand;
            Assert.NotNull(result);
            Assert.Equal(showIndex, result.ShowIndex);
            Assert.Equal(showStatus, result.ShowStatus);
            Assert.Equal(showDate, result.ShowDate);
            Assert.Equal(showAll, result.ShowAll);
        }

        [Fact]
        public void Parse_ViewCommand_WithInvalidFlag_ThrowsInvalidCommandException()
        {
            Assert.Throws<InvalidCommandException>(() => CommandParser.Parse("view -x"));
        }

        [Theory]
        [InlineData("add Buy milk", "Buy milk", false)]
        [InlineData("add \"Buy milk\"", "Buy milk", false)]
        [InlineData("add -m", "", true)]
        [InlineData("add --multiline", "", true)]
        public void Parse_AddCommand_ReturnsCorrectAddCommand(string input, string expectedText, bool expectedMultiline)
        {
            var result = CommandParser.Parse(input) as AddCommand;
            Assert.NotNull(result);
            Assert.Equal(expectedText, result.TaskText);
            Assert.Equal(expectedMultiline, result.IsMultiline);
        }

        [Fact]
        public void Parse_AddCommand_WithoutText_ThrowsInvalidArgumentException()
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse("add"));
        }

        [Theory]
        [InlineData("read 5", 5)]
        [InlineData("read 1", 1)]
        [InlineData("read 100", 100)]
        public void Parse_ReadCommand_WithValidNumber_ReturnsReadCommand(string input, int expectedNumber)
        {
            var result = CommandParser.Parse(input) as ReadCommand;
            Assert.NotNull(result);
            Assert.Equal(expectedNumber, result.TaskNumber);
        }

        [Theory]
        [InlineData("read")]
        [InlineData("read abc")]
        public void Parse_ReadCommand_WithInvalidArguments_ThrowsInvalidArgumentException(string input)
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse(input));
        }

        [Theory]
        [InlineData("delete 5", 5)]
        [InlineData("delete 1", 1)]
        [InlineData("delete 100", 100)]
        public void Parse_DeleteCommand_WithValidNumber_ReturnsDeleteCommand(string input, int expectedNumber)
        {
            var result = CommandParser.Parse(input) as DeleteCommand;
            Assert.NotNull(result);
            Assert.Equal(expectedNumber, result.TaskNumber);
        }

        [Theory]
        [InlineData("delete")]
        [InlineData("delete abc")]
        public void Parse_DeleteCommand_WithInvalidArguments_ThrowsInvalidArgumentException(string input)
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse(input));
        }

        [Theory]
        [InlineData("update 5 New text", 5, "New text")]
        [InlineData("update 1 \"Quoted text\"", 1, "\"Quoted text\"")]
        [InlineData("update 10 Multiple words here", 10, "Multiple words here")]
        public void Parse_UpdateCommand_WithValidArguments_ReturnsUpdateCommand(
            string input, int expectedNumber, string expectedText)
        {
            var result = CommandParser.Parse(input) as UpdateCommand;
            Assert.NotNull(result);
            Assert.Equal(expectedNumber, result.TaskNumber);
            Assert.Equal(expectedText, result.NewText);
        }

        [Theory]
        [InlineData("update")]
        [InlineData("update 5")]
        [InlineData("update abc text")]
        public void Parse_UpdateCommand_WithInvalidArguments_ThrowsInvalidArgumentException(string input)
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse(input));
        }

        [Theory]
        [InlineData("status 5 Completed", 5, TodoStatus.Completed)]
        [InlineData("status 1 InProgress", 1, TodoStatus.InProgress)]
        [InlineData("status 10 notstarted", 10, TodoStatus.NotStarted)]
        public void Parse_StatusCommand_WithValidArguments_ReturnsStatusCommand(
            string input, int expectedNumber, TodoStatus expectedStatus)
        {
            var result = CommandParser.Parse(input) as StatusCommand;
            Assert.NotNull(result);
            Assert.Equal(expectedNumber, result.TaskNumber);
            Assert.Equal(expectedStatus, result.NewStatus);
        }

        [Theory]
        [InlineData("status")]
        [InlineData("status 5")]
        [InlineData("status abc Completed")]
        [InlineData("status 5 InvalidStatus")]
        public void Parse_StatusCommand_WithInvalidArguments_ThrowsInvalidArgumentException(string input)
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse(input));
        }

        [Theory]
        [InlineData("load 3 100", 3, 100)]
        [InlineData("load 1 50", 1, 50)]
        [InlineData("load 10 1000", 10, 1000)]
        public void Parse_LoadCommand_WithValidArguments_ReturnsLoadCommand(
            string input, int expectedCount, int expectedSize)
        {
            var result = CommandParser.Parse(input) as LoadCommand;
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("load")]
        [InlineData("load 3")]
        [InlineData("load abc 100")]
        [InlineData("load 3 abc")]
        [InlineData("load -1 100")]
        [InlineData("load 3 -50")]
        public void Parse_LoadCommand_WithInvalidArguments_ThrowsInvalidArgumentException(string input)
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse(input));
        }

        [Fact]
        public void Parse_SearchCommand_WithContainsFlag_ReturnsSearchCommand()
        {
            var result = CommandParser.Parse("search --contains test") as SearchCommand;
            Assert.NotNull(result);
        }

        [Fact]
        public void Parse_SearchCommand_WithMultipleFlags_ReturnsSearchCommand()
        {
            var result = CommandParser.Parse("search --contains test --status Completed --sort text --desc") as SearchCommand;
            Assert.NotNull(result);
        }

        [Fact]
        public void Parse_SearchCommand_WithInvalidFlag_ThrowsInvalidCommandException()
        {
            Assert.Throws<InvalidCommandException>(() => CommandParser.Parse("search --invalid test"));
        }

        [Fact]
        public void Parse_UnknownCommand_ThrowsInvalidCommandException()
        {
            Assert.Throws<InvalidCommandException>(() => CommandParser.Parse("unknowncommand"));
        }

        [Theory]
        [InlineData("ADD Buy milk")]
        [InlineData("VIEW -i")]
        [InlineData("EXIT")]
        public void Parse_CommandsAreCaseInsensitive(string input)
        {
            var result = CommandParser.Parse(input);
            Assert.NotNull(result);
        }
    }
}
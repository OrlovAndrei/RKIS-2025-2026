using Xunit;
using TodoList;
using TodoList.Exceptions;
using TodoList.Models;

namespace TodoList.Tests
{
    public class CommandParserTests
    {
        [Theory]
        [InlineData("help", typeof(HelpCommand))]
        [InlineData("exit", typeof(ExitCommand))]
        [InlineData("undo", typeof(UndoCommand))]
        [InlineData("redo", typeof(RedoCommand))]
        [InlineData("profile", typeof(ProfileCommand))]
        [InlineData("profile --out", typeof(ProfileCommand))]
        [InlineData("profile -o", typeof(ProfileCommand))]
        [InlineData("add \"Buy milk\"", typeof(AddCommand))]
        [InlineData("add --multiline", typeof(AddCommand))]
        [InlineData("add -m", typeof(AddCommand))]
        [InlineData("view", typeof(ViewCommand))]
        [InlineData("view --index", typeof(ViewCommand))]
        [InlineData("view -i -s", typeof(ViewCommand))]
        [InlineData("view --all", typeof(ViewCommand))]
        [InlineData("read 5", typeof(ReadCommand))]
        [InlineData("status 3 Completed", typeof(StatusCommand))]
        [InlineData("delete 2", typeof(DeleteCommand))]
        [InlineData("update 1 \"New text\"", typeof(UpdateCommand))]
        [InlineData("update 1 --multiline", typeof(UpdateCommand))]
        [InlineData("update 1 -m", typeof(UpdateCommand))]
        [InlineData("search", typeof(SearchCommand))]
        [InlineData("search --contains \"test\"", typeof(SearchCommand))]
        [InlineData("search --from 2026-01-01 --to 2026-12-31", typeof(SearchCommand))]
        [InlineData("search --status Completed --sort text --desc --top 10", typeof(SearchCommand))]
        [InlineData("load 5 100", typeof(LoadCommand))]
        public void Parse_ValidCommand_ReturnsCorrectCommandType(string input, Type expectedType)
        {
            var command = CommandParser.Parse(input);

            Assert.IsType(expectedType, command);
        }

        [Theory]
        [InlineData("add")]
        [InlineData("read")]
        [InlineData("status 1")]
        [InlineData("delete")]
        [InlineData("update 1")]
        [InlineData("load")]
        [InlineData("load 5")]
        [InlineData("search --contains")]
        [InlineData("search --from")]
        [InlineData("search --status")]
        [InlineData("search --sort")]
        [InlineData("search --top")]
        public void Parse_InvalidArguments_ThrowsInvalidArgumentException(string input)
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse(input));
        }

        [Fact]
        public void Parse_UnknownCommand_ThrowsInvalidCommandException()
        {
            Assert.Throws<InvalidCommandException>(() => CommandParser.Parse("unknown"));
        }

        [Fact]
        public void Parse_EmptyString_ThrowsInvalidArgumentException()
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse(""));
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse("   "));
        }

        [Theory]
        [InlineData("read 5", 5)]
        [InlineData("delete 10", 10)]
        public void Parse_ReadDeleteCommand_ParsesIndexCorrectly(string input, int expectedIndex)
        {
            var cmd = CommandParser.Parse(input);

            if (cmd is ReadCommand read)
            {
                var field = typeof(ReadCommand).GetField("_index", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var index = (int)field!.GetValue(read)!;
                Assert.Equal(expectedIndex, index);
            }
            else if (cmd is DeleteCommand delete)
            {
                var field = typeof(DeleteCommand).GetField("_index", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var index = (int)field!.GetValue(delete)!;
                Assert.Equal(expectedIndex, index);
            }
        }

        [Theory]
        [InlineData("status 3 Completed", 3, TodoStatus.Completed)]
        [InlineData("status 1 InProgress", 1, TodoStatus.InProgress)]
        public void Parse_StatusCommand_ParsesCorrectly(string input, int expectedIndex, TodoStatus expectedStatus)
        {
            var cmd = (StatusCommand)CommandParser.Parse(input);

            var indexField = typeof(StatusCommand).GetField("_index", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var statusField = typeof(StatusCommand).GetField("_newStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var index = (int)indexField!.GetValue(cmd)!;
            var status = (TodoStatus)statusField!.GetValue(cmd)!;
            Assert.Equal(expectedIndex, index);
            Assert.Equal(expectedStatus, status);
        }

        [Theory]
        [InlineData("add \"Task with spaces\"", "Task with spaces", false)]
        [InlineData("add --multiline", "", true)]
        [InlineData("add -m", "", true)]
        public void Parse_AddCommand_ParsesCorrectly(string input, string expectedText, bool expectedMultiline)
        {
            var cmd = (AddCommand)CommandParser.Parse(input);

            var textField = typeof(AddCommand).GetField("_text", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var multilineField = typeof(AddCommand).GetField("_isMultiline", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var text = (string)textField!.GetValue(cmd)!;
            var multiline = (bool)multilineField!.GetValue(cmd)!;
            Assert.Equal(expectedText, text);
            Assert.Equal(expectedMultiline, multiline);
        }

        [Theory]
        [InlineData("load 3 50", 3, 50)]
        [InlineData("load 10 100", 10, 100)]
        public void Parse_LoadCommand_ParsesCorrectly(string input, int expectedCount, int expectedSize)
        {
            var cmd = (LoadCommand)CommandParser.Parse(input);

            var countField = typeof(LoadCommand).GetField("_downloadsCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var sizeField = typeof(LoadCommand).GetField("_maxProgress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var count = (int)countField!.GetValue(cmd)!;
            var size = (int)sizeField!.GetValue(cmd)!;
            Assert.Equal(expectedCount, count);
            Assert.Equal(expectedSize, size);
        }

        [Theory]
        [InlineData("search --contains \"hello\" --sort text --desc --top 5", 
            "hello", null, null, null, null, null, "text", true, 5)]
        [InlineData("search --starts-with \"A\" --ends-with \"Z\" --status Completed", 
            null, "A", "Z", null, null, TodoStatus.Completed, null, false, null)]
        [InlineData("search --from 2026-01-01 --to 2026-12-31", 
            null, null, null, "2026-01-01", "2026-12-31", null, null, false, null)]
        public void Parse_SearchCommand_ParsesFlagsCorrectly(
            string input,
            string? expectedContains,
            string? expectedStartsWith,
            string? expectedEndsWith,
            string? fromDateStr,
            string? toDateStr,
            TodoStatus? expectedStatus,
            string? expectedSort,
            bool expectedDesc,
            int? expectedTop)
        {
            var cmd = (SearchCommand)CommandParser.Parse(input);

            var flagsField = typeof(SearchCommand).GetField("_flags", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var flags = (SearchFlags)flagsField!.GetValue(cmd)!;

            Assert.Equal(expectedContains, flags.ContainsText);
            Assert.Equal(expectedStartsWith, flags.StartsWithText);
            Assert.Equal(expectedEndsWith, flags.EndsWithText);

            if (fromDateStr != null)
                Assert.Equal(DateTime.ParseExact(fromDateStr, "yyyy-MM-dd", null), flags.FromDate);
            else
                Assert.Null(flags.FromDate);

            if (toDateStr != null)
                Assert.Equal(DateTime.ParseExact(toDateStr, "yyyy-MM-dd", null), flags.ToDate);
            else
                Assert.Null(flags.ToDate);

            Assert.Equal(expectedStatus, flags.Status);
            Assert.Equal(expectedSort, flags.SortBy);
            Assert.Equal(expectedDesc, flags.Descending);
            Assert.Equal(expectedTop, flags.TopCount);
        }

        [Fact]
        public void Parse_SearchCommand_WithInvalidDate_ThrowsInvalidArgumentException()
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse("search --from 2026/01/01"));
        }

        [Fact]
        public void Parse_SearchCommand_WithInvalidStatus_ThrowsInvalidArgumentException()
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse("search --status Unknown"));
        }

        [Fact]
        public void Parse_SearchCommand_WithInvalidSort_ThrowsInvalidArgumentException()
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse("search --sort length"));
        }

        [Fact]
        public void Parse_SearchCommand_WithNonPositiveTop_ThrowsInvalidArgumentException()
        {
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse("search --top 0"));
            Assert.Throws<InvalidArgumentException>(() => CommandParser.Parse("search --top -5"));
        }
    }
}
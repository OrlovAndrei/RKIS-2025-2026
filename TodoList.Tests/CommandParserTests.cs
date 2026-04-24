using System;
using Xunit;
using TodoList.Commands;
using TodoList.Exceptions;

namespace TodoList.Tests
{
    public class CommandParserTests
    {
        [Theory]
        [InlineData("add \"Купить молоко\"")]
        [InlineData("add --multiline")]
        [InlineData("add -m")]
        public void Parse_AddCommand_ReturnsAddCommand(string input)
        {
            // Act
            var command = CommandParser.Parse(input);

            // Assert
            Assert.IsType<AddCommand>(command);
        }

        [Theory]
        [InlineData("view")]
        [InlineData("view --index")]
        [InlineData("view -i -s")]
        public void Parse_ViewCommand_ReturnsViewCommand(string input)
        {
            // Act
            var command = CommandParser.Parse(input);

            // Assert
            Assert.IsType<ViewCommand>(command);
        }

		[Theory]
		[InlineData("delete 1", "1")]
		[InlineData("delete 5", "5")]
		[InlineData("delete 99", "99")]
		public void Parse_DeleteCommand_ReturnsDeleteCommand(string input, string expectedArg)
		{
			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<DeleteCommand>(command);
			var deleteCommand = command as DeleteCommand;
			Assert.Equal(expectedArg, deleteCommand?.Arg);
		}

        [Theory]
        [InlineData("update 1 Новый текст")]
        [InlineData("update 3 \"Текст в кавычках\"")]
        public void Parse_UpdateCommand_ReturnsUpdateCommand(string input)
        {
            // Act
            var command = CommandParser.Parse(input);

            // Assert
            Assert.IsType<UpdateCommand>(command);
        }

        [Theory]
        [InlineData("read 1")]
        [InlineData("read 7")]
        public void Parse_ReadCommand_ReturnsReadCommand(string input)
        {
            // Act
            var command = CommandParser.Parse(input);

            // Assert
            Assert.IsType<ReadCommand>(command);
        }

        [Theory]
        [InlineData("status 1 Completed")]
        [InlineData("status 2 InProgress")]
        public void Parse_StatusCommand_ReturnsStatusCommand(string input)
        {
            // Act
            var command = CommandParser.Parse(input);

            // Assert
            Assert.IsType<StatusCommand>(command);
        }

        [Fact]
        public void Parse_ProfileCommand_ReturnsProfileCommand()
        {
            // Act
            var command = CommandParser.Parse("profile");

            // Assert
            Assert.IsType<ProfileCommand>(command);
        }

        [Fact]
        public void Parse_ProfileWithOutFlag_ReturnsProfileCommandWithFlag()
        {
            // Act
            var command = CommandParser.Parse("profile --out") as ProfileCommand;

            // Assert
            Assert.IsType<ProfileCommand>(command);
            Assert.Contains("out", command.Flags);
        }

        [Fact]
        public void Parse_HelpCommand_ReturnsHelpCommand()
        {
            // Act
            var command = CommandParser.Parse("help");

            // Assert
            Assert.IsType<HelpCommand>(command);
        }

        [Fact]
        public void Parse_UndoCommand_ReturnsUndoCommand()
        {
            // Act
            var command = CommandParser.Parse("undo");

            // Assert
            Assert.IsType<UndoCommand>(command);
        }

        [Fact]
        public void Parse_RedoCommand_ReturnsRedoCommand()
        {
            // Act
            var command = CommandParser.Parse("redo");

            // Assert
            Assert.IsType<RedoCommand>(command);
        }

        [Theory]
        [InlineData("search --contains \"Купить\"")]
        [InlineData("search --from 2024-01-01 --to 2024-12-31")]
        [InlineData("search --status Completed --sort date --desc --top 5")]
        public void Parse_SearchCommand_ReturnsSearchCommand(string input)
        {
            // Act
            var command = CommandParser.Parse(input);

            // Assert
            Assert.IsType<SearchCommand>(command);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Parse_EmptyInput_ReturnsNull(string input)
        {
            // Act
            var command = CommandParser.Parse(input);

            // Assert
            Assert.Null(command);
        }

        [Theory]
        [InlineData("unknowncommand")]
        [InlineData("blabla")]
        public void Parse_UnknownCommand_ThrowsInvalidCommandException(string input)
        {
            // Act & Assert
            Assert.Throws<InvalidCommandException>(() => CommandParser.Parse(input));
        }
    }
}
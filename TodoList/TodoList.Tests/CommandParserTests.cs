using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using System;

namespace TodoApp.Tests
{
	public class CommandParserTests
	{
		[Fact]
		public void Parse_AddCommand_ReturnsAddCommand()
		{
			// Arrange
			string input = "add Купить молоко";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<AddCommand>(command);
		}

		[Fact]
		public void Parse_AddCommandWithMultipleWords_ReturnsAddCommand()
		{
			// Arrange
			string input = "add Купить хлеб и молоко";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<AddCommand>(command);
		}

		[Fact]
		public void Parse_DeleteCommand_ReturnsDeleteCommand()
		{
			// Arrange
			string input = "delete 1";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<DeleteCommand>(command);
		}

		[Fact]
		public void Parse_DeleteCommandWithAlias_ReturnsDeleteCommand()
		{
			// Arrange
			string input = "del 5";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<DeleteCommand>(command);
		}

		[Fact]
		public void Parse_ViewCommand_ReturnsViewCommand()
		{
			// Arrange
			string input = "view";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<ViewCommand>(command);
		}

		[Fact]
		public void Parse_ViewCommandWithFlags_ReturnsViewCommand()
		{
			// Arrange
			string input = "view --status --date";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<ViewCommand>(command);
		}

		[Fact]
		public void Parse_UpdateCommand_ReturnsUpdateCommand()
		{
			// Arrange
			string input = "update 1 Новая задача";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<UpdateCommand>(command);
		}

		[Fact]
		public void Parse_StatusCommand_ReturnsStatusCommand()
		{
			// Arrange
			string input = "status 1 completed";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<StatusCommand>(command);
		}

		[Fact]
		public void Parse_HelpCommand_ReturnsHelpCommand()
		{
			// Arrange
			string input = "help";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<HelpCommand>(command);
		}

		[Fact]
		public void Parse_HelpCommandWithAlias_ReturnsHelpCommand()
		{
			// Arrange
			string input = "h";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<HelpCommand>(command);
		}

		[Fact]
		public void Parse_ProfileCommand_ReturnsProfileCommand()
		{
			// Arrange
			string input = "profile";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<ProfileCommand>(command);
		}

		[Fact]
		public void Parse_ReadCommand_ReturnsReadCommand()
		{
			// Arrange
			string input = "read 1";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<ReadCommand>(command);
		}

		[Fact]
		public void Parse_UndoCommand_ReturnsUndoCommand()
		{
			// Arrange
			string input = "undo";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<UndoCommand>(command);
		}

		[Fact]
		public void Parse_RedoCommand_ReturnsRedoCommand()
		{
			// Arrange
			string input = "redo";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<RedoCommand>(command);
		}

		[Theory]
		[InlineData("add")]
		[InlineData("delete")]
		[InlineData("update")]
		[InlineData("view")]
		[InlineData("help")]
		[InlineData("profile")]
		[InlineData("undo")]
		[InlineData("redo")]
		public void Parse_ValidCommandStrings_DoesNotThrowException(string commandName)
		{
			// Arrange
			string input = commandName;

			// Act & Assert
			var exception = Record.Exception(() => CommandParser.Parse(input));
			Assert.Null(exception);
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("   ")]
		public void Parse_EmptyOrWhitespaceInput_ThrowsArgumentException(string input)
		{
			// Act & Assert
			Assert.Throws<ArgumentException>(() => CommandParser.Parse(input));
		}

		[Fact]
		public void Parse_NullInput_ThrowsArgumentNullException()
		{
			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => CommandParser.Parse(null));
		}

		[Theory]
		[InlineData("unknown")]
		[InlineData("test")]
		[InlineData("123")]
		[InlineData("!@#$")]
		public void Parse_UnknownCommand_ThrowsException(string input)
		{
			// Act & Assert
			Assert.Throws<Exception>(() => CommandParser.Parse(input));
		}

		[Fact]
		public void Parse_DeleteCommandWithoutIndex_ThrowsException()
		{
			// Arrange
			string input = "delete";

			// Act & Assert
			Assert.Throws<Exception>(() => CommandParser.Parse(input));
		}

		[Fact]
		public void Parse_UpdateCommandWithoutIndexAndText_ThrowsException()
		{
			// Arrange
			string input = "update";

			// Act & Assert
			Assert.Throws<Exception>(() => CommandParser.Parse(input));
		}

		[Fact]
		public void Parse_StatusCommandWithoutIndexAndStatus_ThrowsException()
		{
			// Arrange
			string input = "status";

			// Act & Assert
			Assert.Throws<Exception>(() => CommandParser.Parse(input));
		}

		[Theory]
		[InlineData("add", "Новая задача")]
		[InlineData("a", "Новая задача")]
		public void Parse_AddCommandAliases_ReturnsAddCommand(string alias, string taskText)
		{
			// Arrange
			string input = $"{alias} {taskText}";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<AddCommand>(command);
		}

		[Theory]
		[InlineData("delete", "1")]
		[InlineData("del", "2")]
		[InlineData("d", "3")]
		public void Parse_DeleteCommandAliases_ReturnsDeleteCommand(string alias, string index)
		{
			// Arrange
			string input = $"{alias} {index}";

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<DeleteCommand>(command);
		}

		[Theory]
		[InlineData("view")]
		[InlineData("v")]
		public void Parse_ViewCommandAliases_ReturnsViewCommand(string alias)
		{
			// Arrange
			string input = alias;

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<ViewCommand>(command);
		}

		[Theory]
		[InlineData("help")]
		[InlineData("h")]
		[InlineData("?")]
		public void Parse_HelpCommandAliases_ReturnsHelpCommand(string alias)
		{
			// Arrange
			string input = alias;

			// Act
			var command = CommandParser.Parse(input);

			// Assert
			Assert.IsType<HelpCommand>(command);
		}

		[Fact]
		public void Parse_CaseInsensitiveCommands_WorksCorrectly()
		{
			// Act
			var command1 = CommandParser.Parse("ADD Купить хлеб");
			var command2 = CommandParser.Parse("Delete 1");
			var command3 = CommandParser.Parse("VIEW");
			var command4 = CommandParser.Parse("HeLp");

			// Assert
			Assert.IsType<AddCommand>(command1);
			Assert.IsType<DeleteCommand>(command2);
			Assert.IsType<ViewCommand>(command3);
			Assert.IsType<HelpCommand>(command4);
		}
	}
}
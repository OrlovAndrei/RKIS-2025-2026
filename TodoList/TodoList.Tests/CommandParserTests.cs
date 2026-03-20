using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using System;

namespace TodoList.Tests
{
	public class CommandParserTests
	{
		[Fact]
		public void Parse_AddCommand_ReturnsAddCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "add Купить молоко";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.AddCommand>(command);
		}

		[Fact]
		public void Parse_AddCommandWithMultipleWords_ReturnsAddCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "add Купить хлеб и молоко";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.AddCommand>(command);
		}

		[Fact]
		public void Parse_DeleteCommand_ReturnsDeleteCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "delete 1";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.DeleteCommand>(command);
		}

		[Fact]
		public void Parse_DeleteCommandWithAlias_ReturnsDeleteCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "del 5";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.DeleteCommand>(command);
		}

		[Fact]
		public void Parse_ViewCommand_ReturnsViewCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "view";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.ViewCommand>(command);
		}

		[Fact]
		public void Parse_ViewCommandWithFlags_ReturnsViewCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "view --status --date";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.ViewCommand>(command);
		}

		[Fact]
		public void Parse_UpdateCommand_ReturnsUpdateCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "update 1 Новая задача";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.UpdateCommand>(command);
		}

		[Fact]
		public void Parse_StatusCommand_ReturnsStatusCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "status 1 completed";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.StatusCommand>(command);
		}

		[Fact]
		public void Parse_HelpCommand_ReturnsHelpCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "help";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.HelpCommand>(command);
		}

		[Fact]
		public void Parse_HelpCommandWithAlias_ReturnsHelpCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "h";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.HelpCommand>(command);
		}

		[Fact]
		public void Parse_SearchCommand_ReturnsSearchCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "search задача";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.SearchCommand>(command);
		}

		[Fact]
		public void Parse_LoadCommand_ReturnsLoadCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "load data.json";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.LoadCommand>(command);
		}

		[Fact]
		public void Parse_ProfileCommand_ReturnsProfileCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "profile";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.ProfileCommand>(command);
		}

		[Fact]
		public void Parse_ReadCommand_ReturnsReadCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "read 1";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.ReadCommand>(command);
		}

		[Fact]
		public void Parse_UndoCommand_ReturnsUndoCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "undo";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.UndoCommand>(command);
		}

		[Fact]
		public void Parse_RedoCommand_ReturnsRedoCommand()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "redo";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.RedoCommand>(command);
		}

		[Theory]
		[InlineData("add")]
		[InlineData("delete")]
		[InlineData("update")]
		[InlineData("view")]
		[InlineData("help")]
		[InlineData("search")]
		[InlineData("load")]
		[InlineData("profile")]
		[InlineData("undo")]
		[InlineData("redo")]
		public void Parse_ValidCommandStrings_DoesNotThrowException(string commandName)
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = commandName;

			// Act & Assert
			var exception = Record.Exception(() => parser.Parse(input));
			Assert.Null(exception);
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("   ")]
		public void Parse_EmptyOrWhitespaceInput_ThrowsArgumentException(string input)
		{
			// Arrange
			var parser = new TodoList.CommandParser();

			// Act & Assert
			Assert.Throws<ArgumentException>(() => parser.Parse(input));
		}

		[Fact]
		public void Parse_NullInput_ThrowsArgumentNullException()
		{
			// Arrange
			var parser = new TodoList.CommandParser();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
		}

		[Theory]
		[InlineData("unknown")]
		[InlineData("test")]
		[InlineData("123")]
		[InlineData("!@#$")]
		public void Parse_UnknownCommand_ThrowsException(string input)
		{
			// Arrange
			var parser = new TodoList.CommandParser();

			// Act & Assert
			Assert.Throws<Exception>(() => parser.Parse(input));
		}

		[Fact]
		public void Parse_DeleteCommandWithoutIndex_ThrowsException()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "delete";

			// Act & Assert
			Assert.Throws<Exception>(() => parser.Parse(input));
		}

		[Fact]
		public void Parse_UpdateCommandWithoutIndexAndText_ThrowsException()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "update";

			// Act & Assert
			Assert.Throws<Exception>(() => parser.Parse(input));
		}

		[Fact]
		public void Parse_StatusCommandWithoutIndexAndStatus_ThrowsException()
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = "status";

			// Act & Assert
			Assert.Throws<Exception>(() => parser.Parse(input));
		}

		[Theory]
		[InlineData("add", "Новая задача")]
		[InlineData("a", "Новая задача")]
		public void Parse_AddCommandAliases_ReturnsAddCommand(string alias, string taskText)
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = $"{alias} {taskText}";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.AddCommand>(command);
		}

		[Theory]
		[InlineData("delete", "1")]
		[InlineData("del", "2")]
		[InlineData("d", "3")]
		public void Parse_DeleteCommandAliases_ReturnsDeleteCommand(string alias, string index)
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = $"{alias} {index}";

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.DeleteCommand>(command);
		}

		[Theory]
		[InlineData("view")]
		[InlineData("v")]
		public void Parse_ViewCommandAliases_ReturnsViewCommand(string alias)
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = alias;

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.ViewCommand>(command);
		}

		[Theory]
		[InlineData("help")]
		[InlineData("h")]
		[InlineData("?")]
		public void Parse_HelpCommandAliases_ReturnsHelpCommand(string alias)
		{
			// Arrange
			var parser = new TodoList.CommandParser();
			string input = alias;

			// Act
			var command = parser.Parse(input);

			// Assert
			Assert.IsType<TodoList.HelpCommand>(command);
		}

		[Fact]
		public void Parse_CaseInsensitiveCommands_WorksCorrectly()
		{
			// Arrange
			var parser = new TodoList.CommandParser();

			// Act
			var command1 = parser.Parse("ADD Купить хлеб");
			var command2 = parser.Parse("Delete 1");
			var command3 = parser.Parse("VIEW");
			var command4 = parser.Parse("HeLp");

			// Assert
			Assert.IsType<TodoList.AddCommand>(command1);
			Assert.IsType<TodoList.DeleteCommand>(command2);
			Assert.IsType<TodoList.ViewCommand>(command3);
			Assert.IsType<TodoList.HelpCommand>(command4);
		}
	}
}
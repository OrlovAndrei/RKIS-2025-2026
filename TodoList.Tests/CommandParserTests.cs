using System;
using System.Collections.Generic;
using TodoApp.Commands;
using TodoApp.Exceptions;
using Xunit;
using TodoApp.Models;
namespace TodoList.Tests
{
	public class CommandParserTests
	{
		private readonly CommandParser _parser;
		private readonly TodoApp.Commands.TodoList _todoList;
		private readonly Guid? _profileId;
		private readonly MockDataStorage _storage;
		private class MockDataStorage : IDataStorage
		{
			public List<Profile> Profiles { get; set; } = new List<Profile>();

			public IEnumerable<TodoItem> LoadTodos(Guid profileId)
			{
				return new List<TodoItem>();
			}

			public void SaveTodos(Guid profileId, IEnumerable<TodoItem> todos)
			{
				// Пустая реализация для тестов
			}

			public void SaveProfile(Profile profile)
			{
				// Пустая реализация для тестов
			}

			public Profile? LoadProfile(Guid profileId)
			{
				return null;
			}

			public IEnumerable<Profile> LoadProfiles()
			{
				return Profiles;
			}

			public void SaveProfiles(IEnumerable<Profile> profiles)
			{
				Profiles.Clear();
				Profiles.AddRange(profiles);
			}

			public void DeleteProfile(Guid profileId)
			{
				
			}

			public bool ProfileExists(Guid profileId)
			{
				return true;
			}
		}

		public CommandParserTests()
		{
			_storage = new MockDataStorage();
			_parser = new CommandParser(_storage);
			_todoList = new TodoApp.Commands.TodoList(new List<TodoItem>());
			_profileId = Guid.NewGuid();
		}
		[Theory]
		[InlineData("add Test task")]
		[InlineData("add \"Quoted task\"")]
		public void Parse_AddCommand_ReturnsAddCommand(string input)
		{
			_todoList.Add(new TodoItem("Existing task"));

			var command = _parser.Parse(input, _profileId, _todoList);

			Assert.IsType<AddCommand>(command);
		}
		[Theory]
		[InlineData("delete 1")]
		[InlineData("delete 2")]
		public void Parse_DeleteCommand_ReturnsDeleteCommand(string input)
		{
			_todoList.Add(new TodoItem("Task 1"));
			_todoList.Add(new TodoItem("Task 2"));

			var command = _parser.Parse(input, _profileId, _todoList);

			Assert.IsType<DeleteCommand>(command);
		}
		[Theory]
		[InlineData("read 1")]
		[InlineData("read 2")]
		public void Parse_ReadCommand_ReturnsReadCommand(string input)
		{
			_todoList.Add(new TodoItem("Task 1"));
			_todoList.Add(new TodoItem("Task 2"));

			var command = _parser.Parse(input, _profileId, _todoList);

			Assert.IsType<ReadCommand>(command);
		}
		[Theory]
		[InlineData("status 1 completed", TodoStatus.Completed)]
		[InlineData("status 1 notstarted", TodoStatus.NotStarted)]
		[InlineData("status 1 inprogress", TodoStatus.InProgress)]
		[InlineData("status 1 postponed", TodoStatus.Postponed)]
		[InlineData("status 1 failed", TodoStatus.Failed)]
		public void Parse_StatusCommand_ReturnsStatusCommand(string input, TodoStatus expectedStatus)
		{
			_todoList.Add(new TodoItem("Task 1"));

			var command = _parser.Parse(input, _profileId, _todoList);

			Assert.IsType<StatusCommand>(command);
		}
		[Theory]
		[InlineData("view")]
		[InlineData("view --index")]
		[InlineData("view --status")]
		[InlineData("view --date")]
		[InlineData("view --all")]
		[InlineData("view -i -s -d -a")]
		public void Parse_ViewCommand_ReturnsViewCommand(string input)
		{
			var command = _parser.Parse(input, _profileId, _todoList);

			Assert.IsType<ViewCommand>(command);
		}
		[Theory]
		[InlineData("profile")]
		[InlineData("profile --out")]
		public void Parse_ProfileCommand_ReturnsProfileCommand(string input)
		{
			var command = _parser.Parse(input, _profileId, _todoList);

			Assert.IsType<ProfileCommand>(command);
		}
		[Fact]
		public void Parse_UndoCommand_ReturnsUndoCommand()
		{
			var command = _parser.Parse("undo", _profileId, _todoList);

			Assert.IsType<UndoCommand>(command);
		}
		[Fact]
		public void Parse_RedoCommand_ReturnsRedoCommand()
		{
			var command = _parser.Parse("redo", _profileId, _todoList);

			Assert.IsType<RedoCommand>(command);
		}
		[Fact]
		public void Parse_ExitCommand_ReturnsExitCommand()
		{
			var command = _parser.Parse("exit", _profileId, _todoList);

			Assert.IsType<ExitCommand>(command);
		}
		[Fact]
		public void Parse_HelpCommand_ReturnsHelpCommand()
		{
			var command = _parser.Parse("help", _profileId, _todoList);

			Assert.IsType<HelpCommand>(command);
		}
		[Theory]
		[InlineData("search --contains test")]
		[InlineData("search --starts-with start")]
		[InlineData("search --ends-with end")]
		[InlineData("search --from 2023-01-01 --to 2023-12-31")]
		[InlineData("search --status completed")]
		[InlineData("search --sort date --desc")]
		[InlineData("search --top 5")]
		public void Parse_SearchCommand_ReturnsSearchCommand(string input)
		{
			var command = _parser.Parse(input, _profileId, _todoList);

			Assert.IsType<SearchCommand>(command);
		}
		[Theory]
		[InlineData("load 10 100")]
		[InlineData("load 5 50")]
		public void Parse_LoadCommand_ReturnsLoadCommand(string input)
		{
			var command = _parser.Parse(input, _profileId, _todoList);

			Assert.IsType<LoadCommand>(command);
		}
		[Theory]
		[InlineData("update 1 New text")]
		public void Parse_UpdateCommand_ReturnsUpdateCommand(string input)
		{
			_todoList.Add(new TodoItem("Old text"));

			var command = _parser.Parse(input, _profileId, _todoList);

			Assert.IsType<UpdateCommand>(command);
		}
		[Fact]
		public void Parse_EmptyInput_ThrowsInvalidCommandException()
		{
			var exception = Record.Exception(() =>
				_parser.Parse("", _profileId, _todoList));

			Assert.NotNull(exception);
			Assert.IsType<InvalidCommandException>(exception);
		}
		[Fact]
		public void Parse_WhiteSpaceInput_ThrowsInvalidCommandException()
		{
			var exception = Record.Exception(() =>
				_parser.Parse("   ", _profileId, _todoList));

			Assert.NotNull(exception);
			Assert.IsType<InvalidCommandException>(exception);
		}
		[Fact]
		public void Parse_UnknownCommand_ThrowsInvalidCommandException()
		{
			var exception = Record.Exception(() =>
				_parser.Parse("unknowncommand", _profileId, _todoList));

			Assert.NotNull(exception);
			Assert.IsType<InvalidCommandException>(exception);
		}
		[Theory]
		[InlineData("delete abc")]
		[InlineData("delete")]
		public void Parse_InvalidDeleteCommand_ThrowsInvalidCommandException(string input)
		{
			var exception = Record.Exception(() =>
				_parser.Parse(input, _profileId, _todoList));

			Assert.NotNull(exception);
			Assert.IsType<InvalidCommandException>(exception);
		}
		[Theory]
		[InlineData("read abc")]
		[InlineData("read")]
		public void Parse_InvalidReadCommand_ThrowsInvalidCommandException(string input)
		{
			var exception = Record.Exception(() =>
				_parser.Parse(input, _profileId, _todoList));

			Assert.NotNull(exception);
			Assert.IsType<InvalidCommandException>(exception);
		}
		[Theory]
		[InlineData("status 1 invalidstatus")]
		[InlineData("status")]
		public void Parse_InvalidStatusCommand_ThrowsException(string input)
		{
			_todoList.Add(new TodoItem("Task 1"));

			var exception = Record.Exception(() =>
				_parser.Parse(input, _profileId, _todoList));

			Assert.NotNull(exception);
			Assert.IsType<InvalidArgumentException>(exception);
		}
		[Theory]
		[InlineData("view --invalidflag")]
		public void Parse_ViewCommandWithInvalidFlag_ThrowsInvalidArgumentException(string input)
		{
			var exception = Record.Exception(() =>
				_parser.Parse(input, _profileId, _todoList));

			Assert.NotNull(exception);
			Assert.IsType<InvalidArgumentException>(exception);
		}
		[Fact]
		public void Parse_AddCommandWithMultiline_ReturnsAddCommand()
		{
			var command = _parser.Parse("add --multiline", _profileId, _todoList);

			Assert.IsType<AddCommand>(command);
		}
		[Theory]
		[InlineData("notstarted", TodoStatus.NotStarted)]
		[InlineData("not_started", TodoStatus.NotStarted)]
		[InlineData("not", TodoStatus.NotStarted)]
		[InlineData("inprogress", TodoStatus.InProgress)]
		[InlineData("in_progress", TodoStatus.InProgress)]
		[InlineData("progress", TodoStatus.InProgress)]
		[InlineData("completed", TodoStatus.Completed)]
		[InlineData("complete", TodoStatus.Completed)]
		[InlineData("done", TodoStatus.Completed)]
		[InlineData("postponed", TodoStatus.Postponed)]
		[InlineData("postpone", TodoStatus.Postponed)]
		[InlineData("failed", TodoStatus.Failed)]
		[InlineData("fail", TodoStatus.Failed)]
		public void Parse_StatusCommandWithVariousStatusFormats_WorksCorrectly(string statusString, TodoStatus expectedStatus)
		{
			_todoList.Add(new TodoItem("Task 1"));
			string input = $"status 1 {statusString}";

			var command = _parser.Parse(input, _profileId, _todoList);

			Assert.IsType<StatusCommand>(command);
		}
		[Theory]
		[InlineData("search --from invalid-date")]
		[InlineData("search --to invalid-date")]
		[InlineData("search --status invalid")]
		[InlineData("search --top -5")]
		[InlineData("search --top abc")]
		[InlineData("search --invalidflag test")]
		public void Parse_SearchCommandWithInvalidParameters_ThrowsInvalidArgumentException(string input)
		{
			var exception = Record.Exception(() =>
				_parser.Parse(input, _profileId, _todoList));

			Assert.NotNull(exception);
			Assert.IsType<InvalidArgumentException>(exception);
		}
		[Theory]
		[InlineData("search --contains \"quoted text with spaces\"")]
		[InlineData("search --starts-with \"quoted start\"")]
		[InlineData("search --ends-with \"quoted end\"")]
		public void Parse_SearchCommandWithQuotedText_ReturnsSearchCommand(string input)
		{
			var command = _parser.Parse(input, _profileId, _todoList);

			Assert.IsType<SearchCommand>(command);
		}
		[Fact]
		public void Parse_DeleteCommandWithInvalidIndex_ThrowsInvalidArgumentException()
		{
			_todoList.Add(new TodoItem("Task 1"));

			var exception = Record.Exception(() =>
				_parser.Parse("delete 99", _profileId, _todoList));

			Assert.NotNull(exception);
			Assert.IsType<InvalidArgumentException>(exception);
		}
		[Fact]
		public void Parse_ReadCommandWithInvalidIndex_ThrowsInvalidArgumentException()
		{
			_todoList.Add(new TodoItem("Task 1"));

			var exception = Record.Exception(() =>
				_parser.Parse("read 99", _profileId, _todoList));

			Assert.NotNull(exception);
			Assert.IsType<InvalidArgumentException>(exception);
		}
		[Fact]
		public void Parse_UpdateCommandWithInvalidFormat_ReturnsErrorCommand()
		{
			var command = _parser.Parse("update 1", _profileId, _todoList);

			Assert.IsType<ErrorCommand>(command);
		}
		[Theory]
		[InlineData("load abc 100")]
		[InlineData("load 10 abc")]
		[InlineData("load")]
		[InlineData("load 10")]
		public void Parse_InvalidLoadCommand_ThrowsLoadCommandException(string input)
		{
			var exception = Record.Exception(() =>
				_parser.Parse(input, _profileId, _todoList));

			Assert.NotNull(exception);
			Assert.IsType<LoadCommandException>(exception);
		}
	}
}
using Xunit;
using System;
using System.Collections.Generic;

namespace TodoListTests  // Изменено с TodoList.Tests на TodoListTests
{
    public class CommandParserTests
    {
        private readonly TodoList _todoList;
        private readonly Profile _profile;
        private readonly IDataStorage _storage;

        public CommandParserTests()
        {
            _todoList = new TodoList();
            _profile = new Profile(Guid.NewGuid(), "testUser", "pass", "Test", "User", 1990);
            _storage = new MockDataStorage();

            CommandParser.Initialize(_todoList, _profile, _storage);
        }

        [Theory]
        [InlineData("add \"Buy milk\"")]
        [InlineData("add \"Test task\"")]
        [InlineData("add \"   Task with spaces   \"")]
        public void Parse_WithValidAddCommand_ReturnsAddCommand(string input)
        {
            // Act
            ICommand result = CommandParser.Parse(input);

            // Assert
            Assert.IsType<AddCommand>(result);
        }

        [Theory]
        [InlineData("view")]
        [InlineData("view -i")]
        [InlineData("view -s")]
        [InlineData("view -d")]
        [InlineData("view -a")]
        public void Parse_WithValidViewCommand_ReturnsViewCommand(string input)
        {
            // Act
            ICommand result = CommandParser.Parse(input);

            // Assert
            Assert.IsType<ViewCommand>(result);
        }

        [Theory]
        [InlineData("delete 1")]
        [InlineData("delete 5")]
        [InlineData("delete 100")]
        public void Parse_WithValidDeleteCommand_ReturnsDeleteCommand(string input)
        {
            // Act
            ICommand result = CommandParser.Parse(input);

            // Assert
            Assert.IsType<DeleteCommand>(result);
        }

        [Theory]
        [InlineData("read 1")]
        [InlineData("read 10")]
        [InlineData("read 99")]
        public void Parse_WithValidReadCommand_ReturnsReadCommand(string input)
        {
            // Act
            ICommand result = CommandParser.Parse(input);

            // Assert
            Assert.IsType<ReadCommand>(result);
        }

        [Theory]
        [InlineData("update 1 \"New text\"")]
        [InlineData("update 5 \"Updated task\"")]
        public void Parse_WithValidUpdateCommand_ReturnsUpdateCommand(string input)
        {
            // Act
            ICommand result = CommandParser.Parse(input);

            // Assert
            Assert.IsType<UpdateCommand>(result);
        }

        [Theory]
        [InlineData("status 1 completed")]
        [InlineData("status 2 notstarted")]
        [InlineData("status 3 inprogress")]
        [InlineData("status 4 postponed")]
        [InlineData("status 5 failed")]
        public void Parse_WithValidStatusCommand_ReturnsStatusCommand(string input)
        {
            // Act
            ICommand result = CommandParser.Parse(input);

            // Assert
            Assert.IsType<StatusCommand>(result);
        }

        [Theory]
        [InlineData("undo")]
        [InlineData("redo")]
        [InlineData("help")]
        [InlineData("exit")]
        [InlineData("profile")]
        [InlineData("profile --out")]
        public void Parse_WithSimpleCommands_ReturnsCorrespondingCommand(string input)
        {
            // Act
            ICommand result = CommandParser.Parse(input);

            // Assert
            if (input == "undo") Assert.IsType<UndoCommand>(result);
            else if (input == "redo") Assert.IsType<RedoCommand>(result);
            else if (input == "help") Assert.IsType<HelpCommand>(result);
            else if (input == "exit") Assert.IsType<ExitCommand>(result);
            else if (input == "profile") Assert.IsType<ProfileCommand>(result);
            else if (input == "profile --out") Assert.IsType<ProfileCommand>(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Parse_WithEmptyInput_ThrowsInvalidCommandException(string input)
        {
            // Act & Assert
            Assert.Throws<InvalidCommandException>(() => CommandParser.Parse(input));
        }

        [Theory]
        [InlineData("unknowncommand")]
        [InlineData("invalid")]
        [InlineData("test123")]
        public void Parse_WithUnknownCommand_ThrowsInvalidCommandException(string input)
        {
            // Act & Assert
            Assert.Throws<InvalidCommandException>(() => CommandParser.Parse(input));
        }
    }

    // Mock для IDataStorage, чтобы не использовать реальные файлы/БД/API
    public class MockDataStorage : IDataStorage
    {
        public void SaveProfiles(IEnumerable<Profile> profiles) { }
        public IEnumerable<Profile> LoadProfiles() => new List<Profile>();
        public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos) { }
        public IEnumerable<TodoItem> LoadTodos(Guid userId) => new List<TodoItem>();
    }
}
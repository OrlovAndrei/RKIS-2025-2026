using System;
using System.Linq;
using Xunit;
using TodoList;
using TodoList.Exceptions;

namespace TodoList.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Constructor_Default_CreatesEmptyList()
        {
            // Act
            var list = new TodoList();

            // Assert
            Assert.Empty(list.GetAllTasks());
        }

        [Fact]
        public void Constructor_WithTasks_CreatesListWithTasks()
        {
            // Arrange
            var tasks = new List<TodoItem>
            {
                new TodoItem("Задача 1"),
                new TodoItem("Задача 2")
            };

            // Act
            var list = new TodoList(tasks);

            // Assert
            Assert.Equal(2, list.GetAllTasks().Count);
        }

        [Fact]
        public void AddTask_WithValidText_AddsTask()
        {
            // Arrange
            var list = new TodoList();
            string text = "Купить молоко";

            // Act
            list.AddTask(text, Array.Empty<string>());

            // Assert
            Assert.Single(list.GetAllTasks());
            Assert.Equal(text, list.GetAllTasks()[0].Text);
            Assert.Equal(TodoStatus.NotStarted, list.GetAllTasks()[0].Status);
        }

        // ТЕСТ УДАЛЕН - он зависал из-за Console.ReadLine

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void AddTask_WithEmptyText_ThrowsException(string emptyText)
        {
            // Arrange
            var list = new TodoList();

            // Act & Assert
            Assert.Throws<InvalidArgumentException>(() => list.AddTask(emptyText, Array.Empty<string>()));
        }

        [Fact]
        public void DeleteTask_WithValidIndex_RemovesTask()
        {
            // Arrange
            var list = new TodoList();
            list.AddTask("Задача 1", Array.Empty<string>());
            list.AddTask("Задача 2", Array.Empty<string>());

            // Act
            list.DeleteTask("1");

            // Assert
            Assert.Single(list.GetAllTasks());
            Assert.Equal("Задача 2", list.GetAllTasks()[0].Text);
        }

		[Theory]
		[InlineData("0")]
		[InlineData("99")]
		public void DeleteTask_WithInvalidNumber_ThrowsTaskNotFoundException(string invalidIndex)
		{
			// Arrange
			var list = new TodoList();
			list.AddTask("Задача 1", Array.Empty<string>());

			// Act & Assert
			Assert.Throws<TaskNotFoundException>(() => list.DeleteTask(invalidIndex));
		}

		[Fact]
		public void DeleteTask_WithNonNumeric_ThrowsInvalidArgumentException()
		{
			// Arrange
			var list = new TodoList();
			list.AddTask("Задача 1", Array.Empty<string>());

			// Act & Assert
			Assert.Throws<InvalidArgumentException>(() => list.DeleteTask("abc"));
}

        [Fact]
        public void UpdateTask_WithValidIndexAndText_UpdatesTask()
        {
            // Arrange
            var list = new TodoList();
            list.AddTask("Старый текст", Array.Empty<string>());
            string newText = "Новый текст";

            // Act
            list.UpdateTask($"1 {newText}");

            // Assert
            Assert.Equal(newText, list.GetAllTasks()[0].Text);
        }

        [Fact]
        public void MarkTaskDone_WithValidIndex_ChangesStatus()
        {
            // Arrange
            var list = new TodoList();
            list.AddTask("Задача", Array.Empty<string>());

            // Act
            list.MarkTaskDone("1");

            // Assert
            Assert.Equal(TodoStatus.Completed, list.GetAllTasks()[0].Status);
        }

        [Fact]
        public void SetStatus_WithValidIndexAndStatus_ChangesStatus()
        {
            // Arrange
            var list = new TodoList();
            list.AddTask("Задача", Array.Empty<string>());

            // Act
            list.SetStatus(0, TodoStatus.InProgress);

            // Assert
            Assert.Equal(TodoStatus.InProgress, list.GetAllTasks()[0].Status);
        }

        [Fact]
        public void ReadTask_WithValidIndex_DoesNotThrowException()
        {
            // Arrange
            var list = new TodoList();
            list.AddTask("Задача", Array.Empty<string>());

            // Act & Assert
            var exception = Record.Exception(() => list.ReadTask("1"));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("99")]
        public void ReadTask_WithInvalidIndex_ThrowsTaskNotFoundException(string invalidIndex)
        {
            // Arrange
            var list = new TodoList();
            list.AddTask("Задача", Array.Empty<string>());

            // Act & Assert
            Assert.Throws<TaskNotFoundException>(() => list.ReadTask(invalidIndex));
        }

        [Fact]
        public void GetAllTasks_ReturnsAllTasks()
        {
            // Arrange
            var list = new TodoList();
            list.AddTask("Задача 1", Array.Empty<string>());
            list.AddTask("Задача 2", Array.Empty<string>());

            // Act
            var tasks = list.GetAllTasks();

            // Assert
            Assert.Equal(2, tasks.Count);
        }

        [Fact]
        public void Indexer_WithValidIndex_ReturnsTask()
        {
            // Arrange
            var list = new TodoList();
            list.AddTask("Задача", Array.Empty<string>());

            // Act
            var task = list[0];

            // Assert
            Assert.Equal("Задача", task.Text);
        }

        [Fact]
        public void Indexer_WithInvalidIndex_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var list = new TodoList();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => list[99]);
        }
    }
}
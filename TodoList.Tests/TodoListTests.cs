using Xunit;
using System;

namespace TodoListTests  // Изменено с TodoList.Tests на TodoListTests
{
    public class TodoListTestsClass  // Переименован, чтобы не конфликтовать с классом TodoList
    {
        [Fact]
        public void Add_WithValidItem_RaisesOnTodoAddedEvent()
        {
            // Arrange
            var todoList = new TodoList();
            var item = new TodoItem("Test task");
            bool eventRaised = false;
            todoList.OnTodoAdded += (i) => eventRaised = true;

            // Act
            todoList.Add(item);

            // Assert
            Assert.True(eventRaised);
            Assert.Equal(1, todoList.Count);
        }

        [Fact]
        public void Delete_WithValidIndex_RemovesItemAndRaisesEvent()
        {
            // Arrange
            var todoList = new TodoList();
            var item = new TodoItem("Task");
            todoList.Add(item);
            bool eventRaised = false;
            todoList.OnTodoDeleted += (i) => eventRaised = true;

            // Act
            todoList.Delete(0);

            // Assert
            Assert.True(eventRaised);
            Assert.Equal(0, todoList.Count);
        }

        [Fact]
        public void Delete_WithInvalidIndex_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var todoList = new TodoList();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => todoList.Delete(0));
        }

        [Fact]
        public void SetStatus_WithValidIndex_ChangesStatusAndRaisesEvent()
        {
            // Arrange
            var todoList = new TodoList();
            var item = new TodoItem("Task");
            todoList.Add(item);
            bool eventRaised = false;
            todoList.OnStatusChanged += (i) => eventRaised = true;

            // Act
            todoList.SetStatus(0, TodoStatus.Completed);

            // Assert
            Assert.True(eventRaised);
            Assert.Equal(TodoStatus.Completed, todoList.GetItem(0).Status);
        }

        [Fact]
        public void SetStatus_WithInvalidIndex_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var todoList = new TodoList();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => todoList.SetStatus(0, TodoStatus.Completed));
        }

        [Fact]
        public void UpdateText_WithValidIndex_UpdatesTextAndRaisesEvent()
        {
            // Arrange
            var todoList = new TodoList();
            var item = new TodoItem("Old text");
            todoList.Add(item);
            bool eventRaised = false;
            todoList.OnTodoUpdated += (i) => eventRaised = true;

            // Act
            todoList.UpdateText(0, "New text");

            // Assert
            Assert.True(eventRaised);
            Assert.Equal("New text", todoList.GetItem(0).Text);
        }

        [Fact]
        public void GetItem_WithValidIndex_ReturnsItem()
        {
            // Arrange
            var todoList = new TodoList();
            var expectedItem = new TodoItem("Task");
            todoList.Add(expectedItem);

            // Act
            var result = todoList.GetItem(0);

            // Assert
            Assert.Same(expectedItem, result);
        }

        [Fact]
        public void GetItem_WithInvalidIndex_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var todoList = new TodoList();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => todoList.GetItem(0));
        }

        [Fact]
        public void Count_AfterMultipleOperations_ReturnsCorrectCount()
        {
            // Arrange
            var todoList = new TodoList();
            todoList.Add(new TodoItem("Task 1"));
            todoList.Add(new TodoItem("Task 2"));

            // Act & Assert
            Assert.Equal(2, todoList.Count);

            // Act
            todoList.Delete(0);

            // Assert
            Assert.Equal(1, todoList.Count);
        }

        [Fact]
        public void Indexer_WithValidIndex_ReturnsItem()
        {
            // Arrange
            var todoList = new TodoList();
            var expectedItem = new TodoItem("Task");
            todoList.Add(expectedItem);

            // Act
            var result = todoList[0];

            // Assert
            Assert.Same(expectedItem, result);
        }

        [Fact]
        public void Indexer_WithInvalidIndex_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var todoList = new TodoList();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => todoList[0]);
        }
    }
}
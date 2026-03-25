using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using System;
using System.Collections.Generic;

namespace TodoApp.Tests
{
	public class TodoListTests
	{
		[Fact]
		public void Add_ValidItem_AddsItemToCollection()
		{
			// Arrange
			var todoList = new TodoList();
			var todoItem = new TodoItem("Купить хлеб");

			// Act
			todoList.Add(todoItem);

			// Assert
			int count = 0;
			foreach (var item in todoList)
			{
				count++;
			}
			Assert.Equal(1, count);
			Assert.Equal("Купить хлеб", todoList.GetItem(0).GetText());
		}

		[Fact]
		public void Add_MultipleItems_AddsAllItems()
		{
			// Arrange
			var todoList = new TodoList();
			var item1 = new TodoItem("Задача 1");
			var item2 = new TodoItem("Задача 2");
			var item3 = new TodoItem("Задача 3");

			// Act
			todoList.Add(item1);
			todoList.Add(item2);
			todoList.Add(item3);

			// Assert
			int count = 0;
			foreach (var item in todoList)
			{
				count++;
			}
			Assert.Equal(3, count);
		}

		[Fact]
		public void Delete_ValidIndex_RemovesItem()
		{
			// Arrange
			var todoList = new TodoList();
			var todoItem = new TodoItem("Удаляемая задача");
			todoList.Add(todoItem);

			// Act
			todoList.Delete(0);

			// Assert
			int count = 0;
			foreach (var item in todoList)
			{
				count++;
			}
			Assert.Equal(0, count);
		}

		[Fact]
		public void Delete_ValidIndex_FiresOnTodoDeletedEvent()
		{
			// Arrange
			var todoList = new TodoList();
			var todoItem = new TodoItem("Удаляемая задача");
			todoList.Add(todoItem);
			bool eventFired = false;
			TodoItem deletedItem = null;
			todoList.OnTodoDeleted += (item) =>
			{
				eventFired = true;
				deletedItem = item;
			};

			// Act
			todoList.Delete(0);

			// Assert
			Assert.True(eventFired);
			Assert.Equal("Удаляемая задача", deletedItem.GetText());
		}

		[Fact]
		public void Delete_InvalidIndex_ThrowsArgumentOutOfRangeException()
		{
			// Arrange
			var todoList = new TodoList();
			todoList.Add(new TodoItem("Задача"));

			// Act & Assert
			var exception1 = Assert.Throws<ArgumentOutOfRangeException>(() => todoList.Delete(-1));
			var exception2 = Assert.Throws<ArgumentOutOfRangeException>(() => todoList.Delete(5));

			Assert.Contains("Индекс вне диапазона", exception1.Message);
			Assert.Contains("Индекс вне диапазона", exception2.Message);
		}

		[Fact]
		public void Update_ValidIndex_UpdatesTaskText()
		{
			// Arrange
			var todoList = new TodoList();
			var todoItem = new TodoItem("Старая задача");
			todoList.Add(todoItem);
			string newText = "Новая задача";

			// Act
			todoList.Update(0, newText);

			// Assert
			Assert.Equal(newText, todoList.GetItem(0).GetText());
		}

		[Fact]
		public void Update_ValidIndex_FiresOnTodoUpdatedEvent()
		{
			// Arrange
			var todoList = new TodoList();
			var todoItem = new TodoItem("Старая задача");
			todoList.Add(todoItem);
			bool eventFired = false;
			TodoItem updatedItem = null;
			todoList.OnTodoUpdated += (item) =>
			{
				eventFired = true;
				updatedItem = item;
			};
			string newText = "Новая задача";

			// Act
			todoList.Update(0, newText);

			// Assert
			Assert.True(eventFired);
			Assert.Equal(newText, updatedItem.GetText());
		}

		[Fact]
		public void Update_InvalidIndex_ThrowsArgumentOutOfRangeException()
		{
			// Arrange
			var todoList = new TodoList();
			todoList.Add(new TodoItem("Задача"));

			// Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => todoList.Update(-1, "Новый текст"));
			Assert.Throws<ArgumentOutOfRangeException>(() => todoList.Update(5, "Новый текст"));
		}

		[Fact]
		public void SetStatus_ValidIndex_UpdatesStatus()
		{
			// Arrange
			var todoList = new TodoList();
			var todoItem = new TodoItem("Задача");
			todoList.Add(todoItem);
			TodoStatus newStatus = TodoStatus.Completed;

			// Act
			todoList.SetStatus(0, newStatus);

			// Assert
			Assert.Equal(newStatus, todoList.GetItem(0).GetStatus());
		}

		[Fact]
		public void SetStatus_ValidIndex_FiresOnStatusChangedEvent()
		{
			// Arrange
			var todoList = new TodoList();
			var todoItem = new TodoItem("Задача");
			todoList.Add(todoItem);
			bool eventFired = false;
			TodoItem statusChangedItem = null;
			todoList.OnStatusChanged += (item) =>
			{
				eventFired = true;
				statusChangedItem = item;
			};
			TodoStatus newStatus = TodoStatus.Completed;

			// Act
			todoList.SetStatus(0, newStatus);

			// Assert
			Assert.True(eventFired);
			Assert.Equal(newStatus, statusChangedItem.GetStatus());
		}

		[Fact]
		public void SetStatus_InvalidIndex_ThrowsArgumentOutOfRangeException()
		{
			// Arrange
			var todoList = new TodoList();
			todoList.Add(new TodoItem("Задача"));

			// Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => todoList.SetStatus(-1, TodoStatus.Completed));
			Assert.Throws<ArgumentOutOfRangeException>(() => todoList.SetStatus(5, TodoStatus.Completed));
		}

		[Fact]
		public void GetItem_ValidIndex_ReturnsCorrectItem()
		{
			// Arrange
			var todoList = new TodoList();
			var expectedItem = new TodoItem("Ожидаемая задача");
			todoList.Add(new TodoItem("Другая задача"));
			todoList.Add(expectedItem);
			todoList.Add(new TodoItem("Еще задача"));

			// Act
			var result = todoList.GetItem(1);

			// Assert
			Assert.Equal(expectedItem.GetText(), result.GetText());
		}

		[Fact]
		public void GetItem_InvalidIndex_ThrowsArgumentOutOfRangeException()
		{
			// Arrange
			var todoList = new TodoList();
			todoList.Add(new TodoItem("Задача"));

			// Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => todoList.GetItem(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => todoList.GetItem(5));
		}

		[Fact]
		public void Constructor_InitializesEmptyList()
		{
			// Arrange & Act
			var todoList = new TodoList();

			// Assert
			int count = 0;
			foreach (var item in todoList)
			{
				count++;
			}
			Assert.Equal(0, count);
		}

		[Fact]
		public void Add_FiresOnTodoAddedEvent()
		{
			// Arrange
			var todoList = new TodoList();
			bool eventFired = false;
			TodoItem addedItem = null;
			todoList.OnTodoAdded += (item) =>
			{
				eventFired = true;
				addedItem = item;
			};
			var newItem = new TodoItem("Новая задача");

			// Act
			todoList.Add(newItem);

			// Assert
			Assert.True(eventFired);
			Assert.Equal(newItem, addedItem);
		}

		[Fact]
		public void GetEnumerator_IteratesOverAllItems()
		{
			// Arrange
			var todoList = new TodoList();
			var item1 = new TodoItem("Задача 1");
			var item2 = new TodoItem("Задача 2");
			todoList.Add(item1);
			todoList.Add(item2);
			var items = new List<TodoItem>();

			// Act
			foreach (var item in todoList)
			{
				items.Add(item);
			}

			// Assert
			Assert.Equal(2, items.Count);
			Assert.Contains(item1, items);
			Assert.Contains(item2, items);
		}

		[Fact]
		public void Delete_WhenListIsEmpty_ThrowsException()
		{
			// Arrange
			var todoList = new TodoList();

			// Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => todoList.Delete(0));
		}
	}
}
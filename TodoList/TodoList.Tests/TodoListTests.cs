using System;
using System.Collections.Generic;
using System.Text;


using Xunit;
using TodoList;

namespace TodoList.Tests
{
	public class TodoListTests
	{
		// ========== ТЕСТ 1: AddItem с валидным элементом ==========
		[Fact]
		public void AddItem_ValidItem_ItemIsAdded()
		{
			// Arrange
			var list = new TodoList();

			// Act
			list.AddItem(item);

			// Assert
			Assert.Single(list.Items);
			Assert.Equal("Задача 1", list.Items[0].Title);
		}

		// ========== ТЕСТ 2: AddItem с null ==========
		[Fact]
		public void AddItem_NullItem_ThrowsArgumentNullException()
		{
			// Arrange
			var list = new TodoList();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => list.AddItem(null));
		}

		// ========== ТЕСТ 3: AddItem несколько элементов ==========
		[Fact]
		public void AddItem_MultipleItems_AllItemsAreAdded()
		{
			// Arrange
			var list = new TodoList();
			var item1 = new TodoItem("Задача 1");
			var item2 = new TodoItem("Задача 2");
			var item3 = new TodoItem("Задача 3");

			// Act
			list.AddItem(item1);
			list.AddItem(item2);
			list.AddItem(item3);

			// Assert
			Assert.Equal(3, list.Items.Count);
		}

		// ========== ТЕСТ 4: RemoveItem существующего элемента ==========
		[Fact]
		public void RemoveItem_ExistingItem_ItemIsRemoved()
		{
			// Arrange
			var list = new TodoList();
			var item = new TodoItem("Задача для удаления");
			list.AddItem(item);

			// Act
			list.RemoveItem(item);

			// Assert
			Assert.Empty(list.Items);
		}

		// ========== ТЕСТ 5: RemoveItem несуществующего элемента ==========
		[Fact]
		public void RemoveItem_NonExistingItem_ThrowsInvalidOperationException()
		{
			// Arrange
			var list = new TodoList();
			var item = new TodoItem("Задача");
			var nonExistingItem = new TodoItem("Нет в списке");

			// Act
			list.AddItem(item);

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => list.RemoveItem(nonExistingItem));
		}
	}
}
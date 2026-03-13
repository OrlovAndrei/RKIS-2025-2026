using System;
using System.Collections.Generic;
using System.Text;


using Xunit;
using TodoList;

namespace TodoList.Tests
{
	public class TodoItemTests
	{
		// ========== ТЕСТ 1: Конструктор с валидным названием ==========
		[Fact]
		public void Constructor_ValidTitle_SetsTitleAndNotCompleted()
		{
			// Arrange
			string title = "Купить молоко";

			// Act
			var item = new TodoItem(title);

			// Assert
			Assert.Equal(title, item.Title);
			Assert.False(item.IsCompleted);
		}

		// ========== ТЕСТ 2: Конструктор с пустой строкой ==========
		[Fact]
		public void Constructor_EmptyTitle_ThrowsArgumentException()
		{
			// Arrange
			string emptyTitle = "";

			// Act & Assert
			Assert.Throws<ArgumentException>(() => new TodoItem(emptyTitle));
		}

		// ========== ТЕСТ 3: Конструктор с пробелами ==========
		[Fact]
		public void Constructor_WhitespaceTitle_ThrowsArgumentException()
		{
			// Arrange
			string whitespaceTitle = "   ";

			// Act & Assert
			Assert.Throws<ArgumentException>(() => new TodoItem(whitespaceTitle));
		}

		// ========== ТЕСТ 4: MarkAsCompleted ==========
		[Fact]
		public void MarkAsCompleted_WhenCalled_SetsIsCompletedToTrue()
		{
			// Arrange
			var item = new TodoItem("Тестовая задача");

			// Act
			item.MarkAsCompleted();

			// Assert
			Assert.True(item.IsCompleted);
		}
	}
}
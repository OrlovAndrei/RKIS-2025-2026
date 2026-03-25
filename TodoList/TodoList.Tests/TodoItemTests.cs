using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using System;

namespace TodoApp.Tests
{
	public class TodoItemTests
	{
		[Fact]
		public void MarkDone_WhenCalled_SetsStatusToCompletedAndUpdatesLastUpdate()
		{
			// Arrange
			var todoItem = new TodoItem("Купить хлеб");
			DateTime beforeUpdate = DateTime.Now;

			// Act
			todoItem.MarkDone();

			// Assert
			Assert.True(todoItem.GetIsDone());
			Assert.Equal(TodoStatus.Completed, todoItem.GetStatus());
			Assert.True(todoItem.GetLastUpdate() >= beforeUpdate);
		}

		[Fact]
		public void GetShortInfo_WhenTextIsLong_TruncatesCorrectly()
		{
			// Arrange
			string longText = "Это очень длинный текст задачи, который должен быть обрезан до 30 символов";
			var todoItem = new TodoItem(longText);

			// Act
			string shortInfo = todoItem.GetShortInfo();

			// Assert
			Assert.Equal(30, shortInfo.Length);
			Assert.EndsWith("...", shortInfo);
		}

		[Fact]
		public void GetShortInfo_WhenTextIsShort_ReturnsFullTextWithoutTruncation()
		{
			// Arrange
			string shortText = "Купить молоко";
			var todoItem = new TodoItem(shortText);

			// Act
			string result = todoItem.GetShortInfo();

			// Assert
			Assert.Equal(shortText, result);
		}

		[Theory]
		[InlineData(TodoStatus.NotStarted, "Не начато")]
		[InlineData(TodoStatus.InProgress, "В процессе")]
		[InlineData(TodoStatus.Completed, "Выполнено")]
		[InlineData(TodoStatus.Postponed, "Отложено")]
		[InlineData(TodoStatus.Failed, "Провалено")]
		public void GetStatusText_VariousStatuses_ReturnsCorrectLocalizedString(TodoStatus status, string expectedText)
		{
			// Arrange
			var todoItem = new TodoItem("Тест", status, DateTime.Now);

			// Act
			string result = todoItem.GetStatusText();

			// Assert
			Assert.Equal(expectedText, result);
		}

		[Fact]
		public void UpdateText_WhenCalled_UpdatesTextAndLastUpdate()
		{
			// Arrange
			var todoItem = new TodoItem("Старая задача");
			DateTime beforeUpdate = DateTime.Now;
			string newText = "Новая задача";

			// Act
			todoItem.UpdateText(newText);

			// Assert
			Assert.Equal(newText, todoItem.GetText());
			Assert.True(todoItem.GetLastUpdate() >= beforeUpdate);
		}

		[Fact]
		public void UpdateStatus_WhenCalled_UpdatesStatusAndLastUpdate()
		{
			// Arrange
			var todoItem = new TodoItem("Задача");
			DateTime beforeUpdate = DateTime.Now;
			TodoStatus newStatus = TodoStatus.InProgress;

			// Act
			todoItem.UpdateStatus(newStatus);

			// Assert
			Assert.Equal(newStatus, todoItem.GetStatus());
			Assert.True(todoItem.GetLastUpdate() >= beforeUpdate);
		}

		[Fact]
		public void Constructor_WithOnlyText_SetsStatusToNotStarted()
		{
			// Arrange & Act
			var todoItem = new TodoItem("Новая задача");

			// Assert
			Assert.Equal(TodoStatus.NotStarted, todoItem.GetStatus());
			Assert.False(todoItem.GetIsDone());
		}
	}
}
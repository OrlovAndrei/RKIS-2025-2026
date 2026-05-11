using Moq;
using System;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TodoApp.Tests
{
	public class TodoItemTests
	{
		[Fact]
		public void Constructor_WhenCalled_SetsLastUpdateToCurrentTime()
		{
			// Arrange
			var fixedTime = new DateTime(2026, 4, 9, 10, 30, 0);
			var clockMock = new Mock<IClock>();
			clockMock.Setup(c => c.Now).Returns(fixedTime);

			// Act
			var todoItem = new TodoItem("Test task", clockMock.Object);

			// Assert
			Assert.Equal(fixedTime, todoItem.GetLastUpdate());
			Assert.Equal("Test task", todoItem.GetText());
			Assert.Equal(TodoStatus.NotStarted, todoItem.GetStatus());
		}

		[Fact]
		public void MarkDone_WhenCalled_UpdatesStatusAndLastUpdateTime()
		{
			// Arrange
			var creationTime = new DateTime(2026, 4, 9, 10, 0, 0);
			var updateTime = new DateTime(2026, 4, 9, 11, 0, 0);

			var clockMock = new Mock<IClock>();
			clockMock.SetupSequence(c => c.Now)
				.Returns(creationTime)  // При создании
				.Returns(updateTime);   // При вызове MarkDone()

			var todoItem = new TodoItem("Test task", clockMock.Object);

			// Act
			todoItem.MarkDone();

			// Assert
			Assert.True(todoItem.GetIsDone());
			Assert.Equal(TodoStatus.Completed, todoItem.GetStatus());
			Assert.Equal(updateTime, todoItem.GetLastUpdate());
		}

		[Fact]
		public void UpdateText_WhenCalled_UpdatesTextAndLastUpdateTime()
		{
			// Arrange
			var creationTime = new DateTime(2026, 4, 9, 10, 0, 0);
			var updateTime = new DateTime(2026, 4, 9, 11, 0, 0);

			var clockMock = new Mock<IClock>();
			clockMock.SetupSequence(c => c.Now)
				.Returns(creationTime)  // При создании
				.Returns(updateTime);   // При обновлении текста

			var todoItem = new TodoItem("Initial text", clockMock.Object);

			// Act
			todoItem.UpdateText("Updated text");

			// Assert
			Assert.Equal("Updated text", todoItem.GetText());
			Assert.Equal(updateTime, todoItem.GetLastUpdate());
		}

		[Fact]
		public void UpdateStatus_WhenCalled_UpdatesStatusAndLastUpdateTime()
		{
			// Arrange
			var creationTime = new DateTime(2026, 4, 9, 10, 0, 0);
			var updateTime = new DateTime(2026, 4, 9, 11, 0, 0);

			var clockMock = new Mock<IClock>();
			clockMock.SetupSequence(c => c.Now)
				.Returns(creationTime)  // При создании
				.Returns(updateTime);   // При обновлении статуса

			var todoItem = new TodoItem("Test task", clockMock.Object);

			// Act
			todoItem.UpdateStatus(TodoStatus.InProgress);

			// Assert
			Assert.Equal(TodoStatus.InProgress, todoItem.GetStatus());
			Assert.Equal(updateTime, todoItem.GetLastUpdate());
		}

		[Fact]
		public void GetShortInfo_WithLongDescription_TruncatesTo30Characters()
		{
			// Arrange
			var fixedTime = new DateTime(2026, 4, 9, 12, 0, 0);
			var clockMock = new Mock<IClock>();
			clockMock.Setup(c => c.Now).Returns(fixedTime);

			var longDescription = "This is a very long description that exceeds thirty characters";
			var todoItem = new TodoItem(longDescription, clockMock.Object);

			// Act
			string shortInfo = todoItem.GetShortInfo();

			// Assert
			Assert.Equal(30, shortInfo.Length);
			Assert.EndsWith("...", shortInfo);
		}

		[Fact]
		public void GetShortInfo_WithShortDescription_ReturnsFullText()
		{
			// Arrange
			var fixedTime = new DateTime(2026, 4, 9, 12, 0, 0);
			var clockMock = new Mock<IClock>();
			clockMock.Setup(c => c.Now).Returns(fixedTime);

			var shortDescription = "Short task";
			var todoItem = new TodoItem(shortDescription, clockMock.Object);

			// Act
			string shortInfo = todoItem.GetShortInfo();

			// Assert
			Assert.Equal(shortDescription, shortInfo);
		}

		[Fact]
		public void GetFullInfo_ReturnsFormattedStringWithAllInfo()
		{
			// Arrange
			var fixedTime = new DateTime(2026, 4, 9, 14, 30, 0);
			var clockMock = new Mock<IClock>();
			clockMock.Setup(c => c.Now).Returns(fixedTime);

			var todoItem = new TodoItem("Complete project", clockMock.Object);
			todoItem.UpdateStatus(TodoStatus.InProgress);

			// Act
			string fullInfo = todoItem.GetFullInfo();

			// Assert
			Assert.Contains("Complete project", fullInfo);
			Assert.Contains("В процессе", fullInfo);
			Assert.Contains("09.04.2026 14:30:00", fullInfo);
		}

		[Fact]
		public void GetStatusText_ReturnsCorrectRussianText()
		{
			// Arrange
			var fixedTime = new DateTime(2026, 4, 9, 12, 0, 0);
			var clockMock = new Mock<IClock>();
			clockMock.Setup(c => c.Now).Returns(fixedTime);

			var todoItem = new TodoItem("Test task", clockMock.Object);

			// Act & Assert
			Assert.Equal("Не начато", todoItem.GetStatusText());

			todoItem.UpdateStatus(TodoStatus.InProgress);
			Assert.Equal("В процессе", todoItem.GetStatusText());

			todoItem.MarkDone();
			Assert.Equal("Выполнено", todoItem.GetStatusText());

			todoItem.UpdateStatus(TodoStatus.Postponed);
			Assert.Equal("Отложено", todoItem.GetStatusText());

			todoItem.UpdateStatus(TodoStatus.Failed);
			Assert.Equal("Провалено", todoItem.GetStatusText());
		}

		[Fact]
		public void Constructor_WithoutClockParameter_UsesSystemClock()
		{
			// Arrange & Act
			var todoItem = new TodoItem("Test task");
			var beforeCreation = DateTime.Now;

			// Assert
			Assert.True(todoItem.GetLastUpdate() >= beforeCreation.AddSeconds(-1));
			Assert.True(todoItem.GetLastUpdate() <= DateTime.Now.AddSeconds(1));
		}

		[Fact]
		public void Constructor_WithStatusAndTime_PreservesProvidedValues()
		{
			// Arrange
			var fixedTime = new DateTime(2026, 4, 9, 12, 0, 0);
			var clockMock = new Mock<IClock>();
			clockMock.Setup(c => c.Now).Returns(DateTime.Now); // Не должно использоваться

			var providedTime = new DateTime(2026, 4, 8, 10, 0, 0);

			// Act
			var todoItem = new TodoItem("Test task", TodoStatus.InProgress, providedTime, clockMock.Object);

			// Assert
			Assert.Equal("Test task", todoItem.GetText());
			Assert.Equal(TodoStatus.InProgress, todoItem.GetStatus());
			Assert.Equal(providedTime, todoItem.GetLastUpdate());
		}
	}
}
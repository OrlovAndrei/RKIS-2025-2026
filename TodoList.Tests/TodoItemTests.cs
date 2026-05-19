using System;
using Xunit;
using Moq;
using ToDoList;

namespace TodoList.Tests
{
	public class TodoItemTests
	{
		[Fact]
		public void SetStatus_WhenCalled_StatusChanges()
		{
			TodoItem item = new TodoItem("Задача Ильи");

			item.SetStatus(TodoStatus.Completed);

			Assert.Equal(TodoStatus.Completed, item.Status);
		}

		[Theory]
		[InlineData("Купить молоко", "Купить молоко")]
		[InlineData("Очень длинная задача Ильи на сегодня", "Очень длинная задача Ильи на с...")]
		public void GetShortInfo_DifferentTexts_ReturnsExpectedStart(string text, string expected)
		{
			TodoItem item = new TodoItem(text);

			string result = item.GetShortInfo();

			Assert.StartsWith(expected, result);
		}

		[Fact]
		public void Constructor_WhenCreated_SetsLastUpdateToFixedTime()
		{
			var fixedTime = new DateTime(2026, 3, 29, 12, 0, 0);
			var clockMock = new Mock<IClock>();
			clockMock.Setup(c => c.Now).Returns(fixedTime);

			var item = new TodoItem("Задача Ильи", clockMock.Object);

			Assert.Equal(fixedTime, item.LastUpdate);
		}

		[Fact]
		public void SetStatus_WhenCalled_UpdatesLastUpdate()
		{
			var startTime = new DateTime(2026, 3, 29, 10, 0, 0);
			var updateTime = new DateTime(2026, 3, 29, 11, 0, 0);
			var clockMock = new Mock<IClock>();
			clockMock.SetupSequence(c => c.Now)
				.Returns(startTime)
				.Returns(updateTime);

			var item = new TodoItem("Тест", clockMock.Object);

			item.SetStatus(TodoStatus.Completed);

			Assert.Equal(updateTime, item.LastUpdate);
		}
	}
}
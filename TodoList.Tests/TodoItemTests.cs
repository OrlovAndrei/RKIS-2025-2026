using System;
using Xunit;
using Moq;
using TodoApp.Models;
namespace TodoList.Tests
{
	public class TodoItemTests
	{
		[Fact]
		public void Constructor_WithValidText_CreatesItemWithNotStartedStatus()
		{
			var text = "Test task";

			var item = new TodoItem(text);

			Assert.Equal(text, item.Text);
			Assert.Equal(TodoStatus.NotStarted, item.Status);
			Assert.False(item.IsDone);
			Assert.NotEqual(default(DateTime), item.CreationDate);
			Assert.NotEqual(default(DateTime), item.LastUpdate);
		}
		[Fact]
		public void Constructor_WithEmptyText_ThrowsArgumentException()
		{
			var emptyText = "";
			var whiteSpaceText = "   ";

			Assert.Throws<ArgumentException>(() => new TodoItem(emptyText));
			Assert.Throws<ArgumentException>(() => new TodoItem(whiteSpaceText));
		}
		[Fact]
		public void Constructor_WithStatusAndCreationDate_CreatesItemWithSpecifiedValues()
		{
			var text = "Test task";
			var creationDate = new DateTime(2023, 1, 1);
			var status = TodoStatus.InProgress;

			var item = new TodoItem(text, false, creationDate, status);

			Assert.Equal(text, item.Text);
			Assert.Equal(status, item.Status);
			Assert.Equal(creationDate, item.CreationDate);
		}
		[Fact]
		public void IsDone_SetToTrue_UpdatesStatusToCompleted()
		{
			var item = new TodoItem("Test task");

			item.MarkDone();

			Assert.Equal(TodoStatus.Completed, item.Status);
			Assert.True(item.IsDone);
		}
		[Fact]
		public void IsDone_SetToFalseWhenCompleted_ChangesStatusToNotStarted()
		{
			var item = new TodoItem("Test task");
			item.MarkDone();

			item.Status = TodoStatus.NotStarted;

			Assert.Equal(TodoStatus.NotStarted, item.Status);
			Assert.False(item.IsDone);
		}
		[Fact]
		public void MarkDone_WhenCalled_MarksTaskAsCompletedAndReturnsMessage()
		{
			var item = new TodoItem("Test task");

			var result = item.MarkDone();

			Assert.True(item.IsDone);
			Assert.Equal("Задача отмечена выполненной: Test task", result);
		}
		[Fact]
		public void UpdateText_WithValidText_UpdatesTextAndTimestamp()
		{
			var item = new TodoItem("Old text");
			var oldTimestamp = item.LastUpdate;
			var newText = "New text";

			item.UpdateText(newText);

			Assert.Equal(newText, item.Text);
			Assert.True(item.LastUpdate >= oldTimestamp);
		}
		[Theory]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData(null)]
		public void UpdateText_WithInvalidText_ThrowsArgumentException(string invalidText)
		{
			var item = new TodoItem("Original text");

			Assert.Throws<ArgumentException>(() => item.UpdateText(invalidText));
		}
		[Theory]
		[InlineData(TodoStatus.NotStarted, "Не начата")]
		[InlineData(TodoStatus.InProgress, "В процессе")]
		[InlineData(TodoStatus.Completed, "Выполнена")]
		[InlineData(TodoStatus.Postponed, "Отложена")]
		[InlineData(TodoStatus.Failed, "Провалена")]
		public void GetStatusDisplayName_WithAllStatuses_ReturnsCorrectName(TodoStatus status, string expectedName)
		{
			var displayName = TodoItem.GetStatusDisplayName(status);

			Assert.Equal(expectedName, displayName);
		}
		[Fact]
		public void GetCurrentStatusDisplayName_ReturnsCorrectName()
		{
			var item = new TodoItem("Test task");
			item.Status = TodoStatus.InProgress;

			var displayName = item.GetCurrentStatusDisplayName();

			Assert.Equal("В процессе", displayName);
		}
		[Fact]
		public void GetShortInfo_WithShortText_ReturnsFullText()
		{
			var item = new TodoItem("Short task");
			item.Status = TodoStatus.Completed;

			var shortInfo = item.GetShortInfo();

			Assert.Contains("Short task", shortInfo);
			Assert.Contains("Выполнена", shortInfo);
		}
		[Fact]
		public void GetShortInfo_WithLongText_TruncatesText()
		{
			var longText = new string('a', 40);
			var item = new TodoItem(longText);

			var shortInfo = item.GetShortInfo();

			Assert.Contains("...", shortInfo);
			Assert.True(shortInfo.Length < longText.Length);
		}
		[Fact]
		public void GetFullInfo_ReturnsCompleteInformation()
		{
			var item = new TodoItem("Test task");

			var fullInfo = item.GetFullInfo();

			Assert.Contains("Test task", fullInfo);
			Assert.Contains("Не начата", fullInfo);
			Assert.Contains("Дата изменения", fullInfo);
		}
		[Fact]
		public void GetFormattedInfo_ReturnsCorrectFormat()
		{
			var item = new TodoItem("Test task");
			var creationDate = item.CreationDate;

			var formatted = item.GetFormattedInfo(0);

			Assert.Contains("1.", formatted);
			Assert.Contains("Test task", formatted);
			Assert.Contains(creationDate.ToString("yyyy-MM-ddTHH:mm:ss"), formatted);
			Assert.Contains("notstarted", formatted);
		}
		[Fact]
		public void GetIndex_WithValidList_ReturnsCorrectIndex()
		{
			var item1 = new TodoItem("Task 1");
			var item2 = new TodoItem("Task 2");
			var list = new List<TodoItem> { item1, item2 };

			var index = item2.GetIndex(list);

			Assert.Equal(1, index);
		}
		[Fact]
		public void SetLastUpdate_UpdatesTimestamp()
		{
			var item = new TodoItem("Test task");
			var newDate = new DateTime(2023, 12, 31, 23, 59, 59);

			item.SetLastUpdate(newDate);

			Assert.Equal(newDate, item.LastUpdate);
		}
		[Fact]
		public void SetStatus_Called_UpdatesLastUpdate()
		{
			var firstTime = new DateTime(2025, 1, 1);
			var secondTime = new DateTime(2025, 1, 2);

			var clockMock = new Mock<IClock>();
			clockMock
				.SetupSequence(c => c.Now)
				.Returns(firstTime)
				.Returns(firstTime)
				.Returns(secondTime);

			var item = new TodoItem("Test task", clock: clockMock.Object);

			item.SetStatus(TodoStatus.InProgress);

			Assert.Equal(TodoStatus.InProgress, item.Status);
			Assert.Equal(secondTime, item.LastUpdate);
		}
		[Fact]
		public void Constructor_Called_SetsFixedCreationDate()
		{
			var fixedTime = new DateTime(2025, 1, 1);

			var clockMock = new Mock<IClock>();

			clockMock
				.Setup(c => c.Now)
				.Returns(fixedTime);

			var item = new TodoItem("Test", clock: clockMock.Object);

			Assert.Equal(fixedTime, item.CreationDate);
			Assert.Equal(fixedTime, item.LastUpdate);
		}

		[Fact]
		public void UpdateText_Called_UpdatesLastUpdate()
		{
			var firstTime = new DateTime(2025, 1, 1);
			var secondTime = new DateTime(2025, 1, 2);

			var clockMock = new Mock<IClock>();

			clockMock
				.SetupSequence(c => c.Now)
				.Returns(firstTime)
				.Returns(secondTime);

			var item = new TodoItem("Old", clock: clockMock.Object);

			item.UpdateText("New");

			Assert.Equal(secondTime, item.LastUpdate);
		}
	}
}

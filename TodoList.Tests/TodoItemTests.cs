using System;
using Moq;
using Xunit;
using TodoList;
using TodoList.Models;

namespace TodoList.Tests
{
    public class TodoItemTests
    {
        private static readonly DateTime FixedNow = new DateTime(2025, 4, 1, 12, 0, 0);

        private Mock<IClock> CreateClockMock(DateTime? now = null)
        {
            var mock = new Mock<IClock>();
            mock.Setup(c => c.Now).Returns(now ?? FixedNow);
            return mock;
        }

        [Fact]
        public void Constructor_WithText_SetsTextDefaultStatusAndLastUpdateFromClock()
        {
            var clockMock = CreateClockMock();
            var text = "Buy milk";

            var item = new TodoItem(text, clockMock.Object);

            Assert.Equal(text, item.Text);
            Assert.Equal(TodoStatus.NotStarted, item.Status);
            Assert.Equal(FixedNow, item.LastUpdate);
        }

        [Fact]
        public void Constructor_WithTextStatusLastUpdate_SetsProperties()
        {
            var text = "Buy milk";
            var status = TodoStatus.Completed;
            var lastUpdate = new DateTime(2023, 1, 1, 12, 0, 0);

            var item = new TodoItem(text, status, lastUpdate);

            Assert.Equal(text, item.Text);
            Assert.Equal(status, item.Status);
            Assert.Equal(lastUpdate, item.LastUpdate);
        }

        [Fact]
        public void UpdateText_ChangesTextAndUpdatesLastUpdateFromClock()
        {
            var initialTime = FixedNow;
            var laterTime = initialTime.AddMinutes(10);
            var clockMock = new Mock<IClock>();
            clockMock.SetupSequence(c => c.Now)
                     .Returns(initialTime)
                     .Returns(laterTime);

            var item = new TodoItem("Old", clockMock.Object);
            var newText = "New text";

            item.UpdateText(newText);

            Assert.Equal(newText, item.Text);
            Assert.Equal(laterTime, item.LastUpdate);
        }

        [Fact]
        public void SetStatus_ChangesStatusAndUpdatesLastUpdateFromClock()
        {
            var initialTime = FixedNow;
            var laterTime = initialTime.AddMinutes(10);
            var clockMock = new Mock<IClock>();
            clockMock.SetupSequence(c => c.Now)
                     .Returns(initialTime)
                     .Returns(laterTime);

            var item = new TodoItem("Task", clockMock.Object);
            var newStatus = TodoStatus.InProgress;

            item.SetStatus(newStatus);

            Assert.Equal(newStatus, item.Status);
            Assert.Equal(laterTime, item.LastUpdate);
        }

        [Theory]
        [InlineData("Short", "[NotStarted] (01.04.2025 12:00)")]
        [InlineData("This is a very long text that exceeds thirty characters by far",
                    "This is a very long text ... [NotStarted] (01.04.2025 12:00)")]
        public void GetShortInfo_ReturnsFormattedString(string text, string expected)
        {
            var clockMock = CreateClockMock();
            var item = new TodoItem(text, clockMock.Object);

            var shortInfo = item.GetShortInfo();

            Assert.Equal(expected, shortInfo);
        }

        [Fact]
        public void GetFullInfo_ReturnsFormattedString()
        {
            var text = "Buy milk";
            var status = TodoStatus.Completed;
            var date = new DateTime(2025, 3, 7, 14, 30, 0);
            var item = new TodoItem(text, status, date);
            var expected = $"Текст: {text}\nСтатус: {status}\nДата последнего изменения: {date:dd.MM.yyyy HH:mm}";

            var fullInfo = item.GetFullInfo();

            Assert.Equal(expected, fullInfo);
        }
    }
}
using System;
using Moq;
using Xunit;
using Todolist;
using Todolist.Models;

namespace Todolist.Tests
{
    public class TodoItemTests
    {
        private static readonly DateTime FixedTime = new DateTime(2025, 3, 15, 12, 0, 0);

        private Mock<IClock> CreateClockMock(DateTime? time = null)
        {
            var mock = new Mock<IClock>();
            mock.Setup(c => c.Now).Returns(time ?? FixedTime);
            return mock;
        }

        [Fact]
        public void Constructor_WithText_SetsProperties()
        {
            var text = "Buy milk";
            var clockMock = CreateClockMock();
            var expectedTime = clockMock.Object.Now;

            var item = new TodoItem(text, clockMock.Object);

            Assert.Equal(text, item.Text);
            Assert.Equal(TodoStatus.NotStarted, item.Status);
            Assert.Equal(expectedTime, item.LastUpdate);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsProperties()
        {
            var text = "Buy milk";
            var status = TodoStatus.InProgress;
            var lastUpdate = new DateTime(2026, 3, 15, 10, 30, 0);
            var clockMock = CreateClockMock();

            var item = new TodoItem(text, clockMock.Object)
            {
                Status = status,
                LastUpdate = lastUpdate
            };

            Assert.Equal(text, item.Text);
            Assert.Equal(status, item.Status);
            Assert.Equal(lastUpdate, item.LastUpdate);
        }

        [Fact]
        public void SetStatus_WithUpdateTimeTrue_UpdatesStatusAndLastUpdate()
        {
            var clockMock = CreateClockMock();
            var item = new TodoItem("Test", clockMock.Object);
            var newTime = FixedTime.AddHours(1);
            clockMock.Setup(c => c.Now).Returns(newTime);

            item.SetStatus(TodoStatus.InProgress);

            Assert.Equal(TodoStatus.InProgress, item.Status);
            Assert.Equal(newTime, item.LastUpdate);
        }

        [Fact]
        public void SetStatus_WithDifferentStatuses_UpdatesCorrectly()
        {
            var clockMock = CreateClockMock();
            var item = new TodoItem("Test", clockMock.Object);
            var statuses = new[] { TodoStatus.InProgress, TodoStatus.Completed, TodoStatus.Postponed, TodoStatus.Failed };

            foreach (var status in statuses)
            {
                var newTime = item.LastUpdate.AddMinutes(10);
                clockMock.Setup(c => c.Now).Returns(newTime);

                item.SetStatus(status);

                Assert.Equal(status, item.Status);
                Assert.Equal(newTime, item.LastUpdate);
            }
        }

        [Fact]
        public void UpdateText_ChangesTextAndLastUpdate()
        {
            var clockMock = CreateClockMock();
            var item = new TodoItem("Old text", clockMock.Object);
            var newTime = FixedTime.AddMinutes(30);
            clockMock.Setup(c => c.Now).Returns(newTime);

            item.UpdateText("New text");

            Assert.Equal("New text", item.Text);
            Assert.Equal(newTime, item.LastUpdate);
        }

        [Fact]
        public void SetLastUpdate_WithDateTime_SetsLastUpdate()
        {
            var clockMock = CreateClockMock();
            var item = new TodoItem("Test", clockMock.Object);
            var newDate = new DateTime(2025, 1, 1);

            item.LastUpdate = newDate;

            Assert.Equal(newDate, item.LastUpdate);
        }

        [Theory]
        [InlineData("Short text", "Short text")]
        [InlineData("This is a very long text that should be truncated because it exceeds thirty characters", "This is a very long text t...")]
        [InlineData("Text with\nnew line", "Text with new line")]
        public void GetShortInfo_ReturnsTruncatedText(string input, string expected)
        {
            var clockMock = CreateClockMock();
            var item = new TodoItem(input, clockMock.Object);

            var result = item.ShortText;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetFullInfo_ReturnsFormattedString()
        {
            var clockMock = CreateClockMock();
            var item = new TodoItem("Buy milk", clockMock.Object)
            {
                Status = TodoStatus.InProgress,
                LastUpdate = new DateTime(2026, 3, 15, 10, 30, 0)
            };

            var result = item.GetFullInfo();

            Assert.Contains("Текст: Buy milk", result);
            Assert.Contains("Статус: InProgress", result);
            Assert.Contains("Последнее изменение: 2026-03-15 10:30:00", result);
        }

        [Fact]
        public void MultipleStatusChanges_KeepHistory()
        {
            var clockMock = CreateClockMock();
            var item = new TodoItem("Test", clockMock.Object);
            
            var time1 = FixedTime.AddHours(1);
            clockMock.Setup(c => c.Now).Returns(time1);
            item.SetStatus(TodoStatus.InProgress);
            
            var time2 = FixedTime.AddHours(2);
            clockMock.Setup(c => c.Now).Returns(time2);
            item.SetStatus(TodoStatus.Completed);

            Assert.Equal(TodoStatus.Completed, item.Status);
            Assert.Equal(time2, item.LastUpdate);
            Assert.True(time2 > time1);
        }

        [Fact]
        public void Constructor_WithoutClock_UsesSystemClock()
        {
            var item = new TodoItem("Test");
            var now = DateTime.Now;

            Assert.True((now - item.LastUpdate).Duration() < TimeSpan.FromSeconds(1));
        }
    }
}
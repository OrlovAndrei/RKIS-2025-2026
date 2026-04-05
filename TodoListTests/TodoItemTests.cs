using System;
using Xunit;

namespace Todolist.Tests
{
    public class TodoItemTests
    {
        [Fact]
        public void Constructor_WithText_SetsProperties()
        {
            var text = "Buy milk";

            var item = new TodoItem(text);

            Assert.Equal(text, item.Text);
            Assert.Equal(TodoStatus.NotStarted, item.Status);
            Assert.True(DateTime.Now - item.LastUpdate < TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsProperties()
        {
            var text = "Buy milk";
            var status = TodoStatus.InProgress;
            var lastUpdate = new DateTime(2026, 3, 15, 10, 30, 0);

            var item = new TodoItem(text, status, lastUpdate);

            Assert.Equal(text, item.Text);
            Assert.Equal(status, item.Status);
            Assert.Equal(lastUpdate, item.LastUpdate);
        }

        [Fact]
        public void SetStatus_WithUpdateTimeTrue_UpdatesStatusAndLastUpdate()
        {
            var item = new TodoItem("Test");
            var oldLastUpdate = item.LastUpdate;
            System.Threading.Thread.Sleep(10);

            item.SetStatus(TodoStatus.InProgress);

            Assert.Equal(TodoStatus.InProgress, item.Status);
            Assert.True(item.LastUpdate > oldLastUpdate);
        }

        [Fact]
        public void SetStatus_WithUpdateTimeFalse_UpdatesStatusOnly()
        {
            var item = new TodoItem("Test");
            var oldLastUpdate = item.LastUpdate;

            item.SetStatus(TodoStatus.InProgress, false);

            Assert.Equal(TodoStatus.InProgress, item.Status);
            Assert.Equal(oldLastUpdate, item.LastUpdate);
        }

        [Theory]
        [InlineData(TodoStatus.NotStarted)]
        [InlineData(TodoStatus.InProgress)]
        [InlineData(TodoStatus.Completed)]
        [InlineData(TodoStatus.Postponed)]
        [InlineData(TodoStatus.Failed)]
        public void SetStatus_WithDifferentStatuses_UpdatesCorrectly(TodoStatus status)
        {
            var item = new TodoItem("Test");

            item.SetStatus(status);

            Assert.Equal(status, item.Status);
        }

        [Fact]
        public void UpdateText_ChangesTextAndLastUpdate()
        {
            var item = new TodoItem("Old text");
            var oldLastUpdate = item.LastUpdate;
            System.Threading.Thread.Sleep(10);

            item.UpdateText("New text");

            Assert.Equal("New text", item.Text);
            Assert.True(item.LastUpdate > oldLastUpdate);
        }

        [Fact]
        public void SetLastUpdate_WithDateTime_SetsLastUpdate()
        {
            var item = new TodoItem("Test");
            var newDate = new DateTime(2025, 1, 1);

            item.SetLastUpdate(newDate);

            Assert.Equal(newDate, item.LastUpdate);
        }

        [Theory]
        [InlineData("Short text", "Short text")]
        [InlineData("This is a very long text that should be truncated because it exceeds thirty characters", "This is a very long text t... ")]
        [InlineData("Text with\nnew line", "Text with new line")]
        public void GetShortInfo_ReturnsTruncatedText(string input, string expected)
        {
            var item = new TodoItem(input);

            var result = item.GetShortInfo();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetFullInfo_ReturnsFormattedString()
        {
            var item = new TodoItem("Buy milk", TodoStatus.InProgress, new DateTime(2026, 3, 15, 10, 30, 0));

            var result = item.GetFullInfo();

            Assert.Contains("Задача: Buy milk", result);
            Assert.Contains("Статус: InProgress", result);
            Assert.Contains("Дата изменения: 15.03.2026 10:30:00", result);
        }

        [Fact]
        public void MultipleStatusChanges_KeepHistory()
        {
            var item = new TodoItem("Test");
            
            item.SetStatus(TodoStatus.InProgress);
            var firstUpdate = item.LastUpdate;
            System.Threading.Thread.Sleep(10);
            
            item.SetStatus(TodoStatus.Completed);
            var secondUpdate = item.LastUpdate;

            Assert.Equal(TodoStatus.Completed, item.Status);
            Assert.True(secondUpdate > firstUpdate);
        }
    }
} 
using System;
using Xunit;
using TodoList;

namespace TodoList.Tests
{
    public class TodoItemTests
    {
        [Fact]
        public void Constructor_WithText_SetsTextAndDefaultStatusAndUpdatesLastUpdate()
        {
            var text = "Buy milk";
            var before = DateTime.Now.AddSeconds(-1);

            var item = new TodoItem(text);

            Assert.Equal(text, item.Text);
            Assert.Equal(TodoStatus.NotStarted, item.Status);
            Assert.True(item.LastUpdate >= before && item.LastUpdate <= DateTime.Now.AddSeconds(1));
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
        public void UpdateText_ChangesTextAndUpdatesLastUpdate()
        {
            var item = new TodoItem("Old");
            var oldLastUpdate = item.LastUpdate;
            var newText = "New text";
            System.Threading.Thread.Sleep(10);

            item.UpdateText(newText);

            Assert.Equal(newText, item.Text);
            Assert.True(item.LastUpdate > oldLastUpdate);
        }

        [Fact]
        public void SetStatus_ChangesStatusAndUpdatesLastUpdate()
        {
            var item = new TodoItem("Task");
            var oldLastUpdate = item.LastUpdate;
            var newStatus = TodoStatus.InProgress;
            System.Threading.Thread.Sleep(10);

            item.SetStatus(newStatus);

            Assert.Equal(newStatus, item.Status);
            Assert.True(item.LastUpdate > oldLastUpdate);
        }

        [Theory]
        [InlineData("Short", "[NotStarted] (dd.MM.yyyy HH:mm)")]
        [InlineData("This is a very long text that exceeds thirty characters by far", "This is a very long text ... [NotStarted] (dd.MM.yyyy HH:mm)")]
        public void GetShortInfo_ReturnsFormattedString(string text, string expectedPattern)
        {
            var fixedDate = new DateTime(2025, 3, 7, 14, 30, 0);
            var item = new TodoItem(text, TodoStatus.NotStarted, fixedDate);
            var expected = expectedPattern.Replace("dd.MM.yyyy HH:mm", fixedDate.ToString("dd.MM.yyyy HH:mm"));

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
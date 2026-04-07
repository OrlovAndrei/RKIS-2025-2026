using Xunit;
using System;

namespace TodoList.Tests.Models
{
    public class TodoItemTests
    {
        [Fact]
        public void Constructor_WithText_InitializesCorrectly()
        {
            var item = new TodoItem("Тестовая задача");

            Assert.Equal("Тестовая задача", item.Text);
            Assert.Equal(TodoStatus.NotStarted, item.Status);
            Assert.True(DateTime.Now - item.LastUpdate < TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void SetStatus_ChangesStatusAndUpdatesDate()
        {
            var item = new TodoItem("Тестовая задача");
            var oldDate = item.LastUpdate;

            System.Threading.Thread.Sleep(10); 
            item.SetStatus(TodoStatus.Completed);

            Assert.Equal(TodoStatus.Completed, item.Status);
            Assert.True(item.LastUpdate > oldDate);
        }

        [Fact]
        public void UpdateText_ChangesTextAndUpdatesDate()
        {
            var item = new TodoItem("Старый текст");
            var oldDate = item.LastUpdate;

            System.Threading.Thread.Sleep(10);
            item.UpdateText("Новый текст");

            Assert.Equal("Новый текст", item.Text);
            Assert.True(item.LastUpdate > oldDate);
        }

        [Theory]
        [InlineData(TodoStatus.NotStarted, "Не начато")]
        [InlineData(TodoStatus.InProgress, "В процессе")]
        [InlineData(TodoStatus.Completed, "Выполнено")]
        [InlineData(TodoStatus.Postponed, "Отложено")]
        [InlineData(TodoStatus.Failed, "Провалено")]
        public void GetStatusDisplay_ReturnsCorrectDisplay(TodoStatus status, string expectedDisplay)
        {
            var item = new TodoItem("Задача");

            item.SetStatus(status);
            var result = item.GetStatusDisplay();

            Assert.Equal(expectedDisplay, result);
        }

        [Fact]
        public void GetFullInfo_ReturnsCompleteInformation()
        {
            var item = new TodoItem("Тестовая задача");
            item.SetStatus(TodoStatus.InProgress);

            var result = item.GetFullInfo();

            Assert.Contains("Тестовая задача", result);
            Assert.Contains("В процессе", result);
        }
    }
}
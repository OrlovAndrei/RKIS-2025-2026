using System;
using Xunit;
using TodoList;

namespace TodoList.Tests
{
    public class TodoItemTests
    {
        [Fact]
        public void Constructor_WithValidText_SetsPropertiesCorrectly()
        {
            // Arrange
            string text = "Купить молоко";
            
            // Act
            var item = new TodoItem(text);
            
            // Assert
            Assert.Equal(text, item.Text);
            Assert.Equal(TodoStatus.NotStarted, item.Status);
            Assert.True(item.LastUpdate <= DateTime.Now);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_WithInvalidText_ThrowsException(string invalidText)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TodoItem(invalidText));
        }

        [Fact]
        public void Constructor_WithStatusAndDate_SetsPropertiesCorrectly()
        {
            // Arrange
            string text = "Тест";
            var status = TodoStatus.InProgress;
            var date = new DateTime(2024, 1, 1);

            // Act
            var item = new TodoItem(text, status, date);

            // Assert
            Assert.Equal(text, item.Text);
            Assert.Equal(status, item.Status);
            Assert.Equal(date, item.LastUpdate);
        }

        [Fact]
        public void MarkDone_WhenNotCompleted_ChangesStatusToCompleted()
        {
            // Arrange
            var item = new TodoItem("Тест");
            var oldDate = item.LastUpdate;

            // Act
            item.MarkDone();

            // Assert
            Assert.Equal(TodoStatus.Completed, item.Status);
            Assert.True(item.LastUpdate >= oldDate);
        }

        [Fact]
        public void MarkDone_WhenAlreadyCompleted_DoesNotChangeDate()
        {
            // Arrange
            var item = new TodoItem("Тест", TodoStatus.Completed, new DateTime(2024, 1, 1));

            // Act
            item.MarkDone();

            // Assert
            Assert.Equal(TodoStatus.Completed, item.Status);
            Assert.Equal(new DateTime(2024, 1, 1), item.LastUpdate);
        }

        [Theory]
        [InlineData(TodoStatus.InProgress)]
        [InlineData(TodoStatus.Completed)]
        [InlineData(TodoStatus.Postponed)]
        [InlineData(TodoStatus.Failed)]
        public void SetStatus_WithValidStatus_ChangesStatus(TodoStatus newStatus)
        {
            // Arrange
            var item = new TodoItem("Тест");
            var oldDate = item.LastUpdate;

            // Act
            item.SetStatus(newStatus);

            // Assert
            Assert.Equal(newStatus, item.Status);
            Assert.True(item.LastUpdate >= oldDate);
        }

        [Fact]
        public void UpdateText_WithNewText_UpdatesTextAndDate()
        {
            // Arrange
            var item = new TodoItem("Старый текст");
            var oldDate = item.LastUpdate;
            string newText = "Новый текст";

            // Act
            item.UpdateText(newText);

            // Assert
            Assert.Equal(newText, item.Text);
            Assert.True(item.LastUpdate >= oldDate);
        }

        [Theory]
        [InlineData("Длинный текст для проверки", 10, "Длинный...")]
        [InlineData("Короткий текст", 20, "Короткий текст")]
        [InlineData("Текст", 4, "Т...")]
        public void GetShortInfo_WithDifferentLengths_ReturnsCorrectString(string text, int maxLen, string expected)
        {
            // Arrange
            var item = new TodoItem(text);

            // Act
            var result = item.GetShortInfo(maxLen);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
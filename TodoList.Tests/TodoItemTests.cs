using Xunit;
using System;

namespace TodoListTests
{
    public class TodoItemTests
    {
        [Fact]
        public void Constructor_WithValidText_CreatesItem()
        {
            // Arrange
            string text = "Buy milk";

            // Act
            var item = new TodoItem(text);

            // Assert
            Assert.Equal(text, item.Text);
            Assert.Equal(TodoStatus.NotStarted, item.Status);
        }

        [Fact]
        public void SetStatus_WithValidStatus_ChangesStatusAndLastUpdate()
        {
            // Arrange
            var item = new TodoItem("Task");
            var beforeUpdate = item.LastUpdate;

            // Act
            item.SetStatus(TodoStatus.Completed);

            // Assert
            Assert.Equal(TodoStatus.Completed, item.Status);
            Assert.NotEqual(beforeUpdate, item.LastUpdate);
        }

        [Fact]
        public void UpdateText_WithValidText_ChangesTextAndLastUpdate()
        {
            // Arrange
            var item = new TodoItem("Old text");
            var beforeUpdate = item.LastUpdate;

            // Act
            item.UpdateText("New text");

            // Assert
            Assert.Equal("New text", item.Text);
            Assert.NotEqual(beforeUpdate, item.LastUpdate);
        }

        [Theory]
        [InlineData(TodoStatus.NotStarted, "Не начато")]
        [InlineData(TodoStatus.InProgress, "В процессе")]
        [InlineData(TodoStatus.Completed, "Выполнено")]
        [InlineData(TodoStatus.Postponed, "Отложено")]
        [InlineData(TodoStatus.Failed, "Провалено")]
        public void GetStatusDisplay_WithVariousStatuses_ReturnsRussianTranslation(TodoStatus status, string expected)
        {
            // Arrange
            var item = new TodoItem("Task");
            item.SetStatus(status);

            // Act
            string result = item.GetStatusDisplay();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetShortInfo_WithLongText_TruncatesTo30Chars()
        {
            // Arrange
            // Создаем текст длиной 50 символов
            string longText = "This is a very long text that should be truncated";
            var item = new TodoItem(longText);
            item.SetStatus(TodoStatus.NotStarted);

            // Act
            string result = item.GetShortInfo();

            // Assert
            // Проверяем, что текст обрезан и содержит "..."
            // Исходный текст 50 символов, должен обрезаться до 30 с "..."
            // Формат вывода: "{Text,-30} {status,-10} {date}"
            // Нас интересует только первая часть - текст
            string textPart = result.Split(' ')[0];

            // Текст должен быть обрезан и содержать "..."
            Assert.Contains("...", result);


            string textWithoutEllipsis = textPart.Replace("...", "");
            Assert.True(textWithoutEllipsis.Length <= 27,
                $"Текст '{textWithoutEllipsis}' имеет длину {textWithoutEllipsis.Length}, ожидалось не более 27");
        }

        [Fact]
        public void GetShortInfo_WithShortText_DoesNotTruncate()
        {
            // Arrange
            string shortText = "Short task";
            var item = new TodoItem(shortText);
            item.SetStatus(TodoStatus.NotStarted);

            // Act
            string result = item.GetShortInfo();

            // Assert
            Assert.Contains(shortText, result);
            Assert.DoesNotContain("...", result);
        }

        [Fact]
        public void GetFullInfo_ReturnsFormattedString()
        {
            // Arrange
            var item = new TodoItem("Test task");

            // Act
            string result = item.GetFullInfo();

            // Assert
            Assert.Contains("Текст:", result);
            Assert.Contains("Статус:", result);
            Assert.Contains("Дата изменения:", result);
        }

        [Fact]
        public void SetLastUpdate_WithCustomDateTime_SetsLastUpdate()
        {
            // Arrange
            var item = new TodoItem("Task");
            var customDate = new DateTime(2024, 1, 1, 12, 0, 0);

            // Act
            item.SetLastUpdate(customDate);

            // Assert
            Assert.Equal(customDate, item.LastUpdate);
        }
    }
}
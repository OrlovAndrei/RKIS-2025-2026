using System;
using Xunit;
using Moq;
using TodoList;
using TodoList.Interfaces;
using TodoList.Models;

namespace TodoList.Tests
{
    public class TodoItemTests
    {
        private readonly Mock<IClock> _clockMock;
        private readonly DateTime _fixedTime;

        public TodoItemTests()
        {
            _clockMock = new Mock<IClock>();
            _fixedTime = new DateTime(2025, 3, 19, 12, 0, 0);
            _clockMock.Setup(c => c.Now).Returns(_fixedTime);
        }

        [Fact]
        public void Constructor_WithValidText_SetsPropertiesCorrectly()
        {
            // Arrange
            string text = "Купить молоко";
            
            // Act
            var item = new TodoItem(text, _clockMock.Object);
            
            // Assert
            Assert.Equal(text, item.Text);
            Assert.Equal(TodoStatus.NotStarted, item.Status);
            Assert.Equal(_fixedTime, item.LastUpdate);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_WithInvalidText_ThrowsException(string invalidText)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TodoItem(invalidText, _clockMock.Object));
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
        public void MarkDone_WhenNotCompleted_ChangesStatusToCompletedAndUpdatesTime()
        {
            // Arrange
            var item = new TodoItem("Тест", _clockMock.Object);
            
            var newTime = _fixedTime.AddHours(1);
            _clockMock.Setup(c => c.Now).Returns(newTime);

            // Act
            item.MarkDone();

            // Assert
            Assert.Equal(TodoStatus.Completed, item.Status);
            Assert.Equal(newTime, item.LastUpdate);
        }

        [Fact]
        public void MarkDone_WhenAlreadyCompleted_DoesNotChangeDate()
        {
            // Arrange
            var item = new TodoItem("Тест", _clockMock.Object);
            item.MarkDone();
            
            var originalTime = item.LastUpdate;
            
            // Act
            item.MarkDone();

            // Assert
            Assert.Equal(TodoStatus.Completed, item.Status);
            Assert.Equal(originalTime, item.LastUpdate);
        }

        [Theory]
        [InlineData(TodoStatus.InProgress)]
        [InlineData(TodoStatus.Completed)]
        [InlineData(TodoStatus.Postponed)]
        [InlineData(TodoStatus.Failed)]
        public void SetStatus_WithValidStatus_ChangesStatusAndUpdatesTime(TodoStatus newStatus)
        {
            // Arrange
            var item = new TodoItem("Тест", _clockMock.Object);
            
            var newTime = _fixedTime.AddHours(2);
            _clockMock.Setup(c => c.Now).Returns(newTime);

            // Act
            item.SetStatus(newStatus);

            // Assert
            Assert.Equal(newStatus, item.Status);
            Assert.Equal(newTime, item.LastUpdate);
        }

        [Fact]
        public void UpdateText_WithNewText_UpdatesTextAndDate()
        {
            // Arrange
            var item = new TodoItem("Старый текст", _clockMock.Object);
            
            var newTime = _fixedTime.AddHours(3);
            _clockMock.Setup(c => c.Now).Returns(newTime);
            string newText = "Новый текст";

            // Act
            item.UpdateText(newText);

            // Assert
            Assert.Equal(newText, item.Text);
            Assert.Equal(newTime, item.LastUpdate);
        }

        [Fact]
        public void UpdateText_WithNull_UpdatesToEmptyStringAndUpdatesDate()
        {
            // Arrange
            var item = new TodoItem("Старый текст", _clockMock.Object);
            
            var newTime = _fixedTime.AddHours(4);
            _clockMock.Setup(c => c.Now).Returns(newTime);

            // Act
            item.UpdateText(null);

            // Assert
            Assert.Equal(string.Empty, item.Text);
            Assert.Equal(newTime, item.LastUpdate);
        }

        [Theory]
        [InlineData("Длинный текст для проверки", 10, "Длинный...")]
        [InlineData("Короткий текст", 20, "Короткий текст")]
        [InlineData("Текст", 4, "Т...")]
        public void GetShortInfo_WithDifferentLengths_ReturnsCorrectString(string text, int maxLen, string expected)
        {
            // Arrange
            var item = new TodoItem(text, _clockMock.Object);

            // Act
            var result = item.GetShortInfo(maxLen);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
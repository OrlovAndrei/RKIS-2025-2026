using Xunit;

namespace TodoListTests  // Изменено с TodoList.Tests на TodoListTests
{
    public class StatusParserTests
    {
        [Theory]
        [InlineData("notstarted", TodoStatus.NotStarted)]
        [InlineData("inprogress", TodoStatus.InProgress)]
        [InlineData("completed", TodoStatus.Completed)]
        [InlineData("postponed", TodoStatus.Postponed)]
        [InlineData("failed", TodoStatus.Failed)]
        public void ParseStatus_WithValidInput_ReturnsCorrespondingStatus(string input, TodoStatus expected)
        {
            // Act
            var result = StatusParser.ParseStatus(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("NOTSTARTED")]
        [InlineData("Complete")]
        public void ParseStatus_WithInvalidInput_ReturnsNull(string input)
        {
            // Act
            var result = StatusParser.ParseStatus(input);
        }

        [Theory]
        [InlineData("notstarted", TodoStatus.NotStarted)]
        [InlineData("inprogress", TodoStatus.InProgress)]
        [InlineData("completed", TodoStatus.Completed)]
        [InlineData("postponed", TodoStatus.Postponed)]
        [InlineData("failed", TodoStatus.Failed)]
        [InlineData("invalid", TodoStatus.NotStarted)]
        [InlineData("", TodoStatus.NotStarted)]
        [InlineData("   ", TodoStatus.NotStarted)]
        public void ParseStatusWithDefault_WithVariousInputs_ReturnsStatusOrDefault(string input, TodoStatus expected)
        {
            // Act
            var result = StatusParser.ParseStatusWithDefault(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("invalid", TodoStatus.Completed, TodoStatus.Completed)]
        [InlineData("", TodoStatus.Postponed, TodoStatus.Postponed)]
        [InlineData("completed", TodoStatus.NotStarted, TodoStatus.Completed)]
        public void ParseStatusWithDefault_CustomDefault_ReturnsCorrectStatus(string input, TodoStatus defaultValue, TodoStatus expected)
        {
            // Act
            var result = StatusParser.ParseStatusWithDefault(input, defaultValue);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
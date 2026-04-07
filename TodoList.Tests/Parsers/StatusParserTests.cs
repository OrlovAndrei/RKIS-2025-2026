using Xunit;

namespace TodoList.Tests.Parsers
{
    public class StatusParserTests
    {
        [Theory]
        [InlineData("notstarted", TodoStatus.NotStarted)]
        [InlineData("inprogress", TodoStatus.InProgress)]
        [InlineData("completed", TodoStatus.Completed)]
        [InlineData("postponed", TodoStatus.Postponed)]
        [InlineData("failed", TodoStatus.Failed)]
        [InlineData("UNKNOWN", null)]
        [InlineData("", null)]
        public void ParseStatus_WithVariousInputs_ReturnsExpectedResult(string input, TodoStatus? expected)
        {

            var result = StatusParser.ParseStatus(input);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("notstarted", TodoStatus.NotStarted)]
        [InlineData("inprogress", TodoStatus.InProgress)]
        [InlineData("completed", TodoStatus.Completed)]
        [InlineData("postponed", TodoStatus.Postponed)]
        [InlineData("failed", TodoStatus.Failed)]
        [InlineData("UNKNOWN", TodoStatus.NotStarted)]
        [InlineData("", TodoStatus.NotStarted)]
        public void ParseStatusWithDefault_WithVariousInputs_ReturnsExpectedResult(
            string input, TodoStatus expected)
        {
            var result = StatusParser.ParseStatusWithDefault(input, TodoStatus.NotStarted);

            Assert.Equal(expected, result);
        }
    }
}
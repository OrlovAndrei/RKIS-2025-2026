using Xunit;
using System;

namespace TodoList.Tests
{
    public class ExceptionTests
    {
        [Fact]
        public void TaskNotFoundException_WithTaskNumber_StoresNumberCorrectly()
        {
            var exception = new TaskNotFoundException(5);

            Assert.Equal(5, exception.TaskNumber);
            Assert.Contains("5", exception.Message);
        }

        [Fact]
        public void InvalidArgumentException_WithParameters_StoresDataCorrectly()
        {
            var exception = new InvalidArgumentException("paramName", "invalid", "reason");

            Assert.Contains("paramName", exception.Message);
            Assert.Contains("invalid", exception.Message);
            Assert.Contains("reason", exception.Message);
        }

        [Fact]
        public void InvalidCommandException_WithCommandAndReason_FormatsMessageCorrectly()
        {
            var exception = new InvalidCommandException("testcmd", "unknown command");

            Assert.Contains("testcmd", exception.Message);
            Assert.Contains("unknown command", exception.Message);
        }
    }
}
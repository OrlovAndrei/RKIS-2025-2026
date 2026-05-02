using Xunit;
using System;

namespace TodoListTests  // Изменено с TodoList.Tests на TodoListTests
{
    public class ProfileTests
    {
        [Fact]
        public void Constructor_WithNullLogin_SetsDefaultUser()
        {
            // Arrange & Act
            var profile = new Profile(Guid.NewGuid(), null, "pass", "John", "Doe", 1990);

            // Assert
            Assert.Equal("user", profile.Login);
        }

        [Fact]
        public void Constructor_WithNullPassword_SetsEmptyString()
        {
            // Arrange & Act
            var profile = new Profile(Guid.NewGuid(), "john", null, "John", "Doe", 1990);

            // Assert
            Assert.Equal("", profile.Password);
        }

        [Fact]
        public void GetInfo_ReturnsFormattedStringWithAge()
        {
            // Arrange
            int birthYear = DateTime.Now.Year - 25;
            var profile = new Profile(Guid.NewGuid(), "john123", "pass", "John", "Doe", birthYear);

            // Act
            string result = profile.GetInfo();

            // Assert
            Assert.Contains("John Doe", result);
            Assert.Contains("возраст 25", result);
            Assert.Contains("логин: john123", result);
        }

        [Fact]
        public void CheckPassword_WithCorrectPassword_ReturnsTrue()
        {
            // Arrange
            var profile = new Profile(Guid.NewGuid(), "user", "secret123", "John", "Doe", 1990);

            // Act
            bool result = profile.CheckPassword("secret123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckPassword_WithIncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var profile = new Profile(Guid.NewGuid(), "user", "secret123", "John", "Doe", 1990);

            // Act
            bool result = profile.CheckPassword("wrongpassword");

            // Assert
            Assert.False(result);
        }
    }
}
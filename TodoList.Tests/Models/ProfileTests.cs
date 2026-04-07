using Xunit;
using System;

namespace TodoList.Tests.Models
{
    public class ProfileTests
    {
        [Fact]
        public void GetInfo_ValidProfile_ReturnsCorrectInfo()
        {
            var id = Guid.NewGuid();
            var profile = new Profile(id, "testuser", "pass123", "Иван", "Петров", 1990);

            var result = profile.GetInfo();

            Assert.Contains("Иван", result);
            Assert.Contains("Петров", result);
            Assert.Contains("testuser", result);
        }

        [Fact]
        public void CheckPassword_CorrectPassword_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var profile = new Profile(id, "testuser", "pass123", "Иван", "Петров", 1990);

            var result = profile.CheckPassword("pass123");

            Assert.True(result);
        }

        [Fact]
        public void CheckPassword_WrongPassword_ReturnsFalse()
        {
            var id = Guid.NewGuid();
            var profile = new Profile(id, "testuser", "pass123", "Иван", "Петров", 1990);

            var result = profile.CheckPassword("wrongpass");

            Assert.False(result);
        }

        [Fact]
        public void Profile_Constructor_InitializesPropertiesCorrectly()
        {
            var id = Guid.NewGuid();

            var profile = new Profile(id, "login", "pass", "Name", "Surname", 2000);

            Assert.Equal(id, profile.Id);
            Assert.Equal("login", profile.Login);
            Assert.Equal("pass", profile.Password);
            Assert.Equal("Name", profile.FirstName);
            Assert.Equal("Surname", profile.LastName);
            Assert.Equal(2000, profile.BirthYear);
        }
    }
}
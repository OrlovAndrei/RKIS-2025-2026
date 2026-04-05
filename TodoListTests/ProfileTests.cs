using System;
using Xunit;

namespace Todolist.Tests
{
    public class ProfileTests
    {
        [Fact]
        public void Constructor_WithValidData_CreatesProfile()
        {
            var login = "testuser";
            var password = "pass123";
            var firstName = "John";
            var lastName = "Doe";
            var birthYear = 1990;

            var profile = new Profile(login, password, firstName, lastName, birthYear);

            Assert.NotEqual(Guid.Empty, profile.Id);
            Assert.Equal(login, profile.Login);
            Assert.Equal(password, profile.Password);
            Assert.Equal(firstName, profile.FirstName);
            Assert.Equal(lastName, profile.LastName);
            Assert.Equal(birthYear, profile.BirthYear);
        }

        [Fact]
        public void Constructor_WithIdAndValidData_CreatesProfileWithSpecifiedId()
        {
            var id = Guid.NewGuid();
            var login = "testuser";
            var password = "pass123";
            var firstName = "John";
            var lastName = "Doe";
            var birthYear = 1990;

            var profile = new Profile(id, login, password, firstName, lastName, birthYear);

            Assert.Equal(id, profile.Id);
            Assert.Equal(login, profile.Login);
            Assert.Equal(password, profile.Password);
            Assert.Equal(firstName, profile.FirstName);
            Assert.Equal(lastName, profile.LastName);
            Assert.Equal(birthYear, profile.BirthYear);
        }

        [Theory]
        [InlineData(1990, 2026, 36)]
        [InlineData(2000, 2026, 26)]
        [InlineData(1985, 2026, 41)]
        public void GetInfo_ReturnsCorrectFormat(int birthYear, int currentYear, int expectedAge)
        {
            var profile = new Profile("login", "pass", "John", "Doe", birthYear);
            
            var result = profile.GetInfo();

            Assert.Equal($"John Doe, возраст {expectedAge} (логин: login)", result);
        }

        [Fact]
        public void CheckPassword_WithCorrectPassword_ReturnsTrue()
        {
            var profile = new Profile("login", "correctPass", "John", "Doe", 1990);

            var result = profile.CheckPassword("correctPass");

            Assert.True(result);
        }

        [Fact]
        public void CheckPassword_WithIncorrectPassword_ReturnsFalse()
        {
            var profile = new Profile("login", "correctPass", "John", "Doe", 1990);

            var result = profile.CheckPassword("wrongPass");

            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithNullOrEmptyLogin_StillCreatesProfile(string invalidLogin)
        {
            var profile = new Profile(invalidLogin, "pass", "John", "Doe", 1990);

            Assert.Equal(invalidLogin, profile.Login);
        }

        [Fact]
        public void GetInfo_ForDifferentProfiles_ReturnsCorrectInfo()
        {
            var profile1 = new Profile("user1", "pass1", "Alice", "Smith", 1995);
            var profile2 = new Profile("user2", "pass2", "Bob", "Johnson", 1988);

            var info1 = profile1.GetInfo();
            var info2 = profile2.GetInfo();

            Assert.Contains("Alice", info1);
            Assert.Contains("Smith", info1);
            Assert.Contains("user1", info1);
            
            Assert.Contains("Bob", info2);
            Assert.Contains("Johnson", info2);
            Assert.Contains("user2", info2);
        }

        [Fact]
        public void Id_IsUniqueForDifferentProfiles()
        {
            var profile1 = new Profile("user1", "pass", "John", "Doe", 1990);
            var profile2 = new Profile("user2", "pass", "Jane", "Smith", 1991);

            Assert.NotEqual(profile1.Id, profile2.Id);
        }
    }
} 
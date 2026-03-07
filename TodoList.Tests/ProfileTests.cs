using System;
using Xunit;
using TodoList;

namespace TodoList.Tests
{
    public class ProfileTests
    {
        [Fact]
        public void Constructor_WithLoginPasswordNameBirthYear_SetsProperties()
        {
            var login = "john";
            var password = "123";
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
            Assert.Equal($"{firstName} {lastName}", profile.Name);
        }

        [Fact]
        public void Constructor_WithId_SetsProperties()
        {
            var id = Guid.NewGuid();
            var login = "john";
            var password = "123";
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

        [Fact]
        public void Constructor_WithSpecificUserData_SetsPropertiesCorrectly()
        {
            var login = "MSSSDI";
            var password = "123"; 
            var firstName = "Артем";
            var lastName = "Бешкеев";
            var birthYear = 2007;

            var profile = new Profile(login, password, firstName, lastName, birthYear);

            Assert.Equal(login, profile.Login);
            Assert.Equal(password, profile.Password);
            Assert.Equal(firstName, profile.FirstName);
            Assert.Equal(lastName, profile.LastName);
            Assert.Equal(birthYear, profile.BirthYear);
            Assert.Equal($"{firstName} {lastName}", profile.Name);
        }

        [Fact]
        public void CheckPassword_WithCorrectPassword_ReturnsTrue()
        {
            var profile = new Profile("user", "pass", "A", "B", 2000);

            var result = profile.CheckPassword("pass");

            Assert.True(result);
        }

        [Fact]
        public void CheckPassword_WithIncorrectPassword_ReturnsFalse()
        {
            var profile = new Profile("user", "pass", "A", "B", 2000);

            var result = profile.CheckPassword("wrong");

            Assert.False(result);
        }

        [Fact]
        public void Update_ChangesProperties()
        {
            var profile = new Profile("user", "pass", "A", "B", 2000);
            var newFirstName = "John";
            var newLastName = "Smith";
            var newBirthYear = 1995;

            profile.Update(newFirstName, newLastName, newBirthYear);

            Assert.Equal(newFirstName, profile.FirstName);
            Assert.Equal(newLastName, profile.LastName);
            Assert.Equal(newBirthYear, profile.BirthYear);
        }

        [Fact]
        public void GetInfo_ReturnsFormattedString()
        {
            var profile = new Profile("johndoe", "pwd", "John", "Doe", 1990);
            var expectedAge = DateTime.Now.Year - 1990;
            var expected = $"John Doe, возраст {expectedAge} (логин: johndoe)";

            var info = profile.GetInfo();

            Assert.Equal(expected, info);
        }

        [Fact]
        public void Age_CalculatesCorrectly()
        {
            var birthYear = 1990;
            var profile = new Profile("u", "p", "F", "L", birthYear);
            var expectedAge = DateTime.Now.Year - birthYear;

            var age = profile.Age;

            Assert.Equal(expectedAge, age);
        }
    }
}
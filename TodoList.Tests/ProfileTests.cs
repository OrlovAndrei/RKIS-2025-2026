using System;
using TodoList;
using Xunit;

namespace TodoList.Tests;

public class ProfileTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesProfile()
    {
        var login = "testuser";
        var password = "pass123";
        var firstName = "Тест";
        var lastName = "Тестов";
        var birthYear = 2000;

        var profile = new Profile(login, password, firstName, lastName, birthYear);

        Assert.Equal(login, profile.Login);
        Assert.Equal(password, profile.Password);
        Assert.Equal(firstName, profile.FirstName);
        Assert.Equal(lastName, profile.LastName);
        Assert.Equal(birthYear, profile.BirthYear);
        Assert.NotEqual(Guid.Empty, profile.Id);
    }

    [Fact]
    public void GetInfo_ReturnsCorrectString()
    {
        var profile = new Profile("user", "pass", "Иван", "Иванов", 1990);
        var expectedAge = DateTime.Now.Year - 1990;
        var expected = $"Иван Иванов, возраст {expectedAge}";

        var info = profile.GetInfo();

        Assert.Equal(expected, info);
    }

    [Fact]
    public void CheckPassword_CorrectPassword_ReturnsTrue()
    {
        var profile = new Profile("user", "secret", "Name", "Last", 2000);
        var result = profile.CheckPassword("secret");
        Assert.True(result);
    }

    [Fact]
    public void CheckPassword_WrongPassword_ReturnsFalse()
    {
        var profile = new Profile("user", "secret", "Name", "Last", 2000);
        var result = profile.CheckPassword("wrong");
        Assert.False(result);
    }
}
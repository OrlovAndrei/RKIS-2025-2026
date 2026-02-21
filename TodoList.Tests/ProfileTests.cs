using System;
using Xunit;
using TodoList;

namespace TodoList.Tests
{
	public class ProfileTests
	{
		[Fact]
		public void Constructor_WithValidData_SetsPropertiesCorrectly()
		{
			// Arrange
			var id = Guid.NewGuid();
			var login = "testuser";
			var password = "password123";
			var firstName = "Тест";
			var lastName = "Пользователь";
			var birthYear = 2000;

			// Act
			var profile = new Profile(id, login, password, firstName, lastName, birthYear);

			// Assert
			Assert.Equal(id, profile.Id);
			Assert.Equal(login, profile.Login);
			Assert.Equal(password, profile.Password);
			Assert.Equal(firstName, profile.FirstName);
			Assert.Equal(lastName, profile.LastName);
			Assert.Equal(birthYear, profile.BirthYear);
		}

		[Fact]
		public void GetAge_WithBirthYear2000_ReturnsCorrectAge()
		{
			// Arrange
			var profile = new Profile(Guid.NewGuid(), "login", "pass", "Имя", "Фамилия", 2000);
			var expectedAge = DateTime.Now.Year - 2000;

			// Act
			var age = profile.GetAge();

			// Assert
			Assert.Equal(expectedAge, age);
		}

		[Fact]
		public void GetAge_WithBirthYear2020_ReturnsCorrectAge()
		{
			// Arrange
			var profile = new Profile(Guid.NewGuid(), "login", "pass", "Имя", "Фамилия", 2020);
			var expectedAge = DateTime.Now.Year - 2020;

			// Act
			var age = profile.GetAge();

			// Assert
			Assert.Equal(expectedAge, age);
		}

		[Fact]
		public void ShowProfile_DoesNotThrowException()
		{
			// Arrange
			var profile = new Profile(Guid.NewGuid(), "login", "pass", "Имя", "Фамилия", 2000);

			// Act & Assert
			var exception = Record.Exception(() => profile.ShowProfile());
			Assert.Null(exception);
		}
	}
}
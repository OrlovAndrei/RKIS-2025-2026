using System;
using System.Collections.Generic;
using System.Text;


using Xunit;
using TodoList; 

namespace TodoList.Tests
{
	public class ProfileTests
	{
		// ========== ТЕСТ 1: IsAdult с возрастом 18 ==========
		[Fact]
		public void IsAdult_AgeAboveOrEqual18_ReturnsTrue_Age18()
		{
			// Arrange
			int currentYear = DateTime.Now.Year;
			int birthYear = currentYear - 18;
			var profile = new Profile("login1", "pass1", "John", "Doe", birthYear);

			// Act
			bool result = profile.IsAdult();

			// Assert
			Assert.True(result);
		}

		// ========== ТЕСТ 2: IsAdult с возрастом больше 18 ==========
		[Fact]
		public void IsAdult_AgeAboveOrEqual18_ReturnsTrue_Age25()
		{
			// Arrange
			int currentYear = DateTime.Now.Year;
			int birthYear = currentYear - 25;
			var profile = new Profile("login1", "pass1", "John", "Doe", birthYear);

			// Act
			bool result = profile.IsAdult();

			// Assert
			Assert.True(result);
		}

		// ========== ТЕСТ 3: IsAdult с возрастом меньше 18 ==========
		[Fact]
		public void IsAdult_AgeBelow18_ReturnsFalse_Age17()
		{
			// Arrange
			int currentYear = DateTime.Now.Year;
			int birthYear = currentYear - 17;
			var profile = new Profile("login1", "pass1", "John", "Doe", birthYear);

			// Act
			bool result = profile.IsAdult();

			// Assert
			Assert.False(result);
		}

		// ========== ТЕСТ 4: IsAdult с возрастом 5 лет ==========
		[Fact]
		public void IsAdult_AgeBelow18_ReturnsFalse_Age5()
		{
			// Arrange
			int currentYear = DateTime.Now.Year;
			int birthYear = currentYear - 5;
			var profile = new Profile("login1", "pass1", "John", "Doe", birthYear);

			// Act
			bool result = profile.IsAdult();

			// Assert
			Assert.False(result);
		}

		// ========== ТЕСТ 5: HasName с некорректными именами ==========
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData("\t")]
		[InlineData("\n")]
		public void HasName_NameIsNullOrWhiteSpace_ReturnsFalse(string firstName)
		{
			// Arrange
			var profile = new Profile("login2", "pass2", firstName, "Smith", 2000);

			// Act
			bool result = profile.HasName();

			// Assert
			Assert.False(result);
		}

		// ========== ТЕСТ 6: HasName с корректными именами ==========
		[Theory]
		[InlineData("John")]
		[InlineData("Alice")]
		[InlineData("Bob")]
		[InlineData("Анна")]
		public void HasName_ValidName_ReturnsTrue(string firstName)
		{
			// Arrange
			var profile = new Profile("login3", "pass3", firstName, "Johnson", 2000);

			// Act
			bool result = profile.HasName();

			// Assert
			Assert.True(result);
		}
	}
}
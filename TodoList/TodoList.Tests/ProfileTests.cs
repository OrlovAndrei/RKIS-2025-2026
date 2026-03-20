using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using System;

namespace TodoList.Tests
{
	public class ProfileTests
	{
		[Fact]
		public void GetInfo_WithValidBirthYear_ReturnsCorrectAgeAndFormat()
		{
			// Arrange
			int birthYear = 1990;
			int currentYear = 2026;
			string firstName = "Иван";
			string lastName = "Петров";
			var profile = new Profile(firstName, lastName, birthYear);

			// Act
			string result = profile.GetInfo(currentYear);

			// Assert
			int expectedAge = currentYear - birthYear;
			string expected = $"({firstName}) ({lastName}), возраст {expectedAge}";
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(2000, 2026, 26)]
		[InlineData(1995, 2026, 31)]
		[InlineData(2026, 2026, 0)]
		[InlineData(2025, 2026, 1)]
		public void GetInfo_VariousBirthYears_ReturnsCorrectAge(int birthYear, int currentYear, int expectedAge)
		{
			// Arrange
			var profile = new Profile("Анна", "Сидорова", birthYear);

			// Act
			string result = profile.GetInfo(currentYear);

			// Assert
			string expected = $"(Анна) (Сидорова), возраст {expectedAge}";
			Assert.Equal(expected, result);
		}
	}
}
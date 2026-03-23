using TodoApp.Commands;
using Xunit;
namespace TodoList.Tests
{
	public class ProfileTests
	{
		[Fact]
		public void Constructor_WithValidParameters_CreatesProfileWithGuid()
		{
			var login = "testtest";
			var password = "234234";
			var firstName = "Kolyan";
			var lastName = "Parker";
			var birthYear = 2003;

			var profile = new Profile(login, password, firstName, lastName, birthYear);

			Assert.NotEqual(Guid.Empty, profile.Id);
			Assert.Equal(login, profile.Login);
			Assert.Equal(password, profile.Password);
			Assert.Equal(firstName, profile.FirstName);
			Assert.Equal(lastName, profile.LastName);
			Assert.Equal(birthYear, profile.BirthYear);
		}
		[Fact]
		public void Age_WhenBirthYearIs2003_ReturnsCorrectAge()
		{

			var profile = new Profile("testtest", "234234", "Kolyan", "Parker", 2003);
			var expectedAge = DateTime.Now.Year - 2003;

			var age = profile.Age;

			Assert.Equal(expectedAge, age);
		}

		[Fact]
		public void GetInfo_WithValidProfile_ReturnsFormattedString()
		{
			var profile = new Profile("Gorb", "secret", "Gege", "Gimblejumb", 1991);

			var info = profile.GetInfo();

			Assert.Contains("Имя:Gege", info);
			Assert.Contains("Фамилия:Gimblejumb", info);
			Assert.Contains($"возраст: {profile.Age}", info);
			Assert.Contains("логин: Gorb", info);
		}

		[Fact]
		public void EmptyConstructor_CreatesEmptyProfile()
		{
			var profile = new Profile();

			Assert.Null(profile.Login);
			Assert.Null(profile.Password);
			Assert.Null(profile.FirstName);
			Assert.Null(profile.LastName);
			Assert.Equal(0, profile.BirthYear);
		}
	}
}
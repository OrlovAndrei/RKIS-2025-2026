using System;

namespace TodoList
{
	public record Profile(
		Guid Id,
		string Login,
		string Password,
		string FirstName,
		string LastName,
		int BirthYear)
	{
		public int GetAge()
		{
			return DateTime.Now.Year - BirthYear;
		}

		public void ShowProfile()
		{
			Console.WriteLine($"{FirstName} {LastName}, {BirthYear} год рождения ({GetAge()} лет)");
		}
	}
}
using System;

namespace Todolist
{
	public class Profile
	{
		private string firstName;
		private string lastName;
		private int birthYear;
		public string FirstName
		{
			get => firstName;
			set => firstName = value;
		}

		public string LastName
		{
			get => lastName;
			set => lastName = value;
		}

		public int BirthYear
		{
			get => birthYear;
			set => birthYear = value;
		}
		public string GetInfo()
		{
			int age = DateTime.Now.Year - BirthYear;
			return $"{FirstName} {LastName}, возраст {age}";
		}
	}
}
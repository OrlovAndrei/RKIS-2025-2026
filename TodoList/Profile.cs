using System;
namespace TodoApp.Commands
{
	public class Profile
	{
		public Guid Id { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int BirthYear { get; set; }
		public int Age => DateTime.Now.Year - BirthYear;
		public Profile(string login, string password, string firstName, string lastName, int birthYear)
		{
			Id = Guid.NewGuid();
			Login = login;
			Password = password;
			FirstName = firstName;
			LastName = lastName;
			BirthYear = birthYear;
		}
		public Profile() { }
		public string GetInfo()
		{
			return $"{FirstName} {LastName} (возраст: {Age}, логин: {Login})";
		}
	}
}
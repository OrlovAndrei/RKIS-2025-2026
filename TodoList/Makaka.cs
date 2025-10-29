using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList;
internal class Makaka
{
	public string FirstName { get; private set; }
	public string LastName { get; private set; }
	public int BirthYear { get; private set; }

	public Makaka(string firstName, string lastName, int birthYear)
	{
		FirstName = firstName;
		LastName = lastName;
		BirthYear = birthYear;
	}

	public string GetInfo()
	{
		int age = DateTime.Now.Year - BirthYear;
		return $"{FirstName} {LastName}, возраст {age}";
	}
}

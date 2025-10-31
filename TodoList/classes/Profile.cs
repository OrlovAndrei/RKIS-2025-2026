namespace TodoList;

public class Profile
{
	public Profile(string firstName, string lastName, int birthYear)
	{
		FirstName = firstName;
		LastName = lastName;
		BirthYear = birthYear;
	}

	public string FirstName { get; }
	public string LastName { get; }
	public int BirthYear { get; }

	public string GetInfo()
	{
		var age = DateTime.Now.Year - BirthYear;
		return $"{FirstName} {LastName}, возраст {age}";
	}
}
public class Profile
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public int BirthYear { get; set; }
	public Profile(string firstName, string lastName, int birthYear)
	{
		FirstName = firstName;
		LastName = lastName;
		BirthYear = birthYear;
	}
	public string GetInfo(int currentYear)
	{
		int age = currentYear - BirthYear;
		return $"{FirstName} {LastName}, возраст {age}";
	}
}

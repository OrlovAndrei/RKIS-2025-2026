public class Profile
{
	private string firstNamee;
	private string lastNamee;
	private int birthYearr;
	public Profile(string firstName, string lastName, int birthYear)
	{
		firstNamee = firstName;
		lastNamee = lastName;
		birthYearr = birthYear;
	}
	public string GetInfo(int currentYear)
	{
		int age = currentYear - birthYearr;
		return $"{firstNamee} {lastNamee}, возраст {age}";
	}
}

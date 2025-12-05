public class Profile
{
	private string _firstName;
	private string _lastName;
	private int _birthYear;
	private string _login;
	private string _password;
	private Guid _id;
	public Profile(string firstName, string lastName, int birthYear)
	{
		_firstName = firstName;
		_lastName = lastName;
		_birthYear = birthYear;
		_id = Guid.NewGuid();
	}
	public string FirstName => _firstName;
	public string LastName => _lastName;
	public int BirthYear => _birthYear;
	public string Login => _login;
	public string Password => _password;
	public Guid Id => _id;
	public string GetInfo(int currentYear)
	{
		int age = currentYear - _birthYear;
		return $"{_firstName} {_lastName}, возраст {age}";
	}
}

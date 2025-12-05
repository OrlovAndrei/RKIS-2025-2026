public class Profile
{
	private string _firstName;
	private string _lastName;
	private int _birthYear;
	private string _login;
	private string _password;
	private Guid _id;
	public Profile(string login, string password, string firstName, string lastName, int birthYear)
	{
		_login = login;
		_password = password;
		_firstName = firstName;
		_lastName = lastName;
		_birthYear = birthYear;
		_id = Guid.NewGuid();
	}
	public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
	{
		_id = id;
		_login = login;
		_password = password;
		_firstName = firstName;
		_lastName = lastName;
		_birthYear = birthYear;
	}
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

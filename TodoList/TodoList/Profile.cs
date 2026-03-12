using System;
using TodoApp.Exceptions;
public class Profile
{
	private string _firstName;
	private string _lastName;
	private int _birthYear;
	private string _login;
	private string _password;
	private Guid _id;
	public Profile()
	{
		_id = Guid.NewGuid();
	}
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
	public string FirstName { get => _firstName; set => _firstName = value; }
	public string LastName { get => _lastName; set => _lastName = value; }
	public int BirthYear { get => _birthYear; set => _birthYear = value; }
	public string Login { get => _login; set => _login = value; }
	public string Password { get => _password; set => _password = value; }
	public Guid Id { get => _id; set => _id = value; }
	public string GetInfo(int currentYear)
	{
		int age = currentYear - _birthYear;
		return $"{_firstName} {_lastName}, возраст {age}";
	}
}

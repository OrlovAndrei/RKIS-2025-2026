namespace TodoApp;
public class Profile
{
    public string FirstName;
    public string LastName;
    public int BirthYear;
    public int Age => DateTime.Now.Year - BirthYear;
    public Profile(string firstName, string lastName, int birthYear)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }
	public Profile()
	{
		FirstName = "User";
		LastName = "Default";
		BirthYear = DateTime.Now.Year - 25; 
	}

	public string GetInfo() => ($"\nДобавлен пользователь: Имя: {FirstName}, Фамилия: {LastName}, Возраст: {Age}");
}
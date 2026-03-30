namespace TodoList;

public class Profile
{
    public Profile(string login, string password, string firstName, string lastName, int birthYear)
    {
        Id = Guid.NewGuid();
        Login = login;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
    {
        Id = id;
        Login = login;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    public Guid Id { get; }
    public string Login { get; }
    public string Password { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public int BirthYear { get; }

    public string GetInfo()
    {
        var age = DateTime.Now.Year - BirthYear;
        return $"{FirstName} {LastName}, возраст {age}";
    }
    
    public bool CheckPassword(string password)
    {
        return Password == password;
    }
}
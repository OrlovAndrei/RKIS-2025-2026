using System;

class Profile
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int BirthYear { get; set; }

    public Profile(string login, string password, string firstName, string lastName, int birthYear)
    {
        Id = Guid.NewGuid();
        Login = login ?? string.Empty;
        Password = password ?? string.Empty;
        FirstName = firstName ?? string.Empty;
        LastName = lastName ?? string.Empty;
        BirthYear = birthYear;
    }

    public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
    {
        Id = id;
        Login = login ?? string.Empty;
        Password = password ?? string.Empty;
        FirstName = firstName ?? string.Empty;
        LastName = lastName ?? string.Empty;
        BirthYear = birthYear;
    }

    public string GetInfo()
    {
        int age = DateTime.Now.Year - BirthYear;
        return $"{(FirstName ?? string.Empty)} {(LastName ?? string.Empty)}, возраст {age} (логин: {Login ?? string.Empty})";
    }
}


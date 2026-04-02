using System;

namespace TodoList;

public class Profile
{
    public Guid Id { get; private set; }
    public string Login { get; private set; }
    public string Password { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int BirthYear { get; private set; }

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

    public bool VerifyPassword(string password) => Password == password;

    public string GetInfo() => $"{FirstName} {LastName} {DateTime.Now.Year - BirthYear}";
}
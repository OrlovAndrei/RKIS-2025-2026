using System;
using System.Collections.Generic;

public class Profile
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int BirthYear { get; set; }

    public List<TodoItem> Todos { get; set; } = new();

    // Конструктор для EF Core
    protected Profile() { }

    public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
    {
        Id = id;
        Login = login ?? "user";
        Password = password ?? "";
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    public string GetInfo()
    {
        int age = DateTime.Now.Year - BirthYear;
        return $"{FirstName} {LastName}, возраст {age}, логин: {Login}";
    }

    public bool CheckPassword(string password) => Password == password;
}
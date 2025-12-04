using System;

class Profile
{
    // Уникальный идентификатор профиля
    public Guid Id { get; set; }

    // Данные для входа
    public string Login { get; set; }
    public string Password { get; set; }

    // Основные персональные данные (оставлены без изменений по заданию)
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int BirthYear { get; set; }

    /// <summary>
    /// Конструктор для создания нового профиля (Id генерируется автоматически).
    /// </summary>
    public Profile(string login, string password, string firstName, string lastName, int birthYear)
    {
        Id = Guid.NewGuid();
        Login = login;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    /// <summary>
    /// Конструктор для загрузки профиля из файла (Id уже известен).
    /// </summary>
    public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
    {
        Id = id;
        Login = login;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    public string GetInfo()
    {
        int age = DateTime.Now.Year - BirthYear;
        return $"{FirstName} {LastName}, возраст {age} (логин: {Login})";
    }
}


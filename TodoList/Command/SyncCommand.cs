using System;

public class SyncCommand : ICommand
{
    public bool IsPull { get; set; }
    public bool IsPush { get; set; }

    public void Execute()
    {
        Console.WriteLine("Данные автоматически сохраняются в SQLite базе данных.");
        Console.WriteLine("Файл БД: todos.db");
        Console.WriteLine("Таблицы: Profiles, Todos");
    }
}
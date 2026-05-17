using System;
using System.Linq;
using TodoList.Exceptions;
using TodoList.Services;

namespace TodoList.commands;

public class SyncCommand : ICommand
{
    public bool IsPull { get; set; }
    public bool IsPush { get; set; }

    public void Execute()
    {
        Console.WriteLine("Синхронизация не поддерживается в текущей версии (переход на SQLite).");
        Console.WriteLine("Используйте локальную базу данных todos.db");
    }

    public void Unexecute() { }
}
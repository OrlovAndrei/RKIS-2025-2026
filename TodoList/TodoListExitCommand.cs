using System;

namespace TodoList
{
    /// <summary>
    /// Команда выхода из приложения.
    /// </summary>
    internal class ExitCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Выход...");
            System.Environment.Exit(0);
        }
    }
}


using System;

namespace TodoList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TodoList todoList = new TodoList();
            Profile profile = new Profile("Иван", "Иванов", 2000);

            Console.WriteLine("TodoList запущен. Введите команду (exit для выхода).");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.ToLower() == "exit")
                    break;

                ICommand command = CommandParser.Parse(input, todoList, profile);

                if (command != null)
                {
                    command.Execute();
                }
                else
                {
                    Console.WriteLine("Неизвестная команда");
                }
            }
        }
    }
}


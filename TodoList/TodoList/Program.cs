using System;

namespace TodoApp
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

                    CommandParser parser = new CommandParser(todoList, profile);
                    ICommand command = parser.Parse(input);

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


using System;

namespace TodoList
{
    class Program
    {
        private static Profile profile;
        private static TodoList todos = new();
        
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Бурнашов и Хазиев");
            profile = CreateUser();
            
            Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");
            Console.WriteLine("Введите 'help' для списка команд");

            while (true)
            {
                string input = Console.ReadLine();

                if (input == null || input.ToLower() == "exit")
                {
                    Console.WriteLine("Выход из программы...");
                    break;
                }

                string command = input.ToLower().Trim();

                if (command == "help")
                {
                    ShowHelp();
                }
                else if (command == "profile")
                {
                    ShowProfile();
                }
                else if (command.StartsWith("add"))
                {
                    AddMultiTask(input);
                }
                else if (command.StartsWith("done"))
                {
                    MarkDoneTask(input);
                }
                else if (command.StartsWith("delete"))
                {
                    DeleteTask(input);
                }
                else if (command.StartsWith("update"))
                {
                    UpdateTask(input);
                }
                else if (command.StartsWith("view"))
                {
                    ViewTasks(input);
                }
                else if (command.StartsWith("read"))
                {
                    ReadTask(input);
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                }
            }
        }

        static Profile CreateUser()
        {
            Console.Write("Введите имя: ");
            var firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            var lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string yearInput = Console.ReadLine();

            int birthYear = int.Parse(yearInput);
            
            return new Profile(firstName, lastName, birthYear);
        }

        static void ViewTasks(string input)
        {
            var flags = ParseFlags(input);

            bool showIndex = flags.Contains("-i") || flags.Contains("--index");
            bool showStatus = flags.Contains("-s") || flags.Contains("--status");
            bool showUpdateDate = flags.Contains("-d") || flags.Contains("--update-date");
            bool showAll = flags.Contains("-a") || flags.Contains("--all");

            todos.View(showIndex, showStatus, showUpdateDate, showAll);
        }
        
        private static void ReadTask(string input)
        {
            var parts = input.Split(' ', 2);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int i) || i < 1)
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи.");
                return;
            }
            
            todos.Read(i - 1);
        }
        
        static void MarkDoneTask(string input)
        {
            var parts = input.Split(' ', 2);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int i) || i < 1)
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи.");
                return;
            }

            todos.MarkDone(i - 1);
        }
        
        static void DeleteTask(string input)
        {
            var parts = input.Split(' ', 2);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int index) || index < 1)
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи.");
                return;
            }
            todos.Delete(index - 1);
        }
        
        static void UpdateTask(string input)
        {
            var parts = input.Split(' ', 3);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int index) || index < 1)
            {
                Console.WriteLine("Ошибка: формат - update <номер> \"новый текст\"");
                return;
            }

            string newText = parts[2].Trim('"');
            todos.Update(index, newText);
        }
    }
}

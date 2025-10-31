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

        static void ShowHelp()
        {
            Console.WriteLine(
            """
            Доступные команды:
            help - вывести список команд
            profile - показать данные пользователя
            add \"текст задачи\" - добавить новую задачу
              -m, --multi — добавить задачу в несколько строк
            done [id] - отметить задачу как выполненную
            delete [id] - удалить задачу
            update [id] \"новый текст\" - обновить текст задачи
            view - показать задачи в табличном виде
              -a, --all — добавить все поля
              -i, --index — добавить индекс
              -s, --status — добавить статус
              -d, --update-date — добавить дату
            read [id] — вывод задачи
            exit - выйти из программы
            """);
        }

        static void ShowProfile()
        {
            Console.WriteLine(profile.GetInfo());
        }

        static void AddMultiTask(string input)
        {
            var flags = ParseFlags(input);
            bool isMultiTask = flags.Contains("-m") || flags.Contains("--multi");
            
            if (isMultiTask)
            {
                string taskText = "";
                Console.WriteLine("Многострочный режим, введите !end для отправки");

                while (true)
                {
                    Console.Write("> ");
                    string line = Console.ReadLine();
                    if (line == "!end") break;
                    taskText += line + "\n";
                }

                AddTask(taskText);
            }
            else
            {
                string taskText = ExtractTaskText(input);
                AddTask(taskText);
            }
        }
        static void AddTask(string taskText)
        {
            if (string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым");
                Console.WriteLine("Формат: add \"текст задачи\"");
                return;
            }

            todos.Add(new TodoItem(taskText));
        }

        static string ExtractTaskText(string input)
        {
            string[] parts = input.Split('"');
            
            if (parts.Length >= 2)
            {
                return parts[1];
            }
            else
            {
                return input.Substring(3).Trim();
            }
        }

        private static string[] ParseFlags(string command)
        {
            var parts = command.Split(' ');
            var flags = new List<string>();

            foreach (var part in parts)
            {
                if (part.StartsWith("--"))
                {
                    flags.Add(part);
                }
                else if (part.StartsWith("-"))
                {
                    for (int i = 1; i < part.Length; i++)
                        flags.Add("-" + part[i]);
                }
            }

            return flags.ToArray();
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

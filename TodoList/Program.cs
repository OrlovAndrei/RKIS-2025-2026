namespace TodoList
{
    class Program
    {
        static string name;
        static string surname;
        static int age;
        public static void Main()
        {
            Console.WriteLine("Работу выполнили: Галстян и Дзуцев");
            CreateUser();

            string[] todos = new string[2];
            bool[] statuses = new bool[2];
            DateTime[] dates = new DateTime[2];
            int index = 0;

            while (true)
            {
                Console.Write("Введите команду: ");
                string input = Console.ReadLine();
                
                if (input == "exit")
                {
                    Console.WriteLine("Выход из программы.");
                    break;
                }
                if (input == "help")
                {
                    HelpCommand();
                }
                else if (input == "profile")
                {
                    ProfileCommand();
                }
                else if (input.StartsWith("add"))
                {
                    AddCommand(input, ref todos, ref statuses, ref dates, ref index);
                }
                else if (input.StartsWith("done "))
                {
                    DoneCommand(input, statuses, dates);
                }
                else if (input.StartsWith("update "))
                {
                    UpdateCommand(input, todos, dates);
                }
                else if (input.StartsWith("delete "))
                {
                    DeleteCommand(input, ref todos, ref statuses, ref dates, ref index);
                }
                else if (input == "view")
                {
                    ViewCommand(todos, statuses, dates);
                }
                else
                {
                    Console.WriteLine("Неизвестная команда.");
                }
            }
        }

        private static void DeleteCommand(string command, ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int index)
        {
            int idx = int.Parse(command.Split(' ')[1]) - 1;
            
            for (int i = idx; i < index - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }
            
            index--;
            todos[index] = string.Empty;
            statuses[index] = false;
            dates[index] = default;
            Console.WriteLine($"Задача под номером {idx + 1} была удалена.");
        }

        private static void UpdateCommand(string command, string[] todos, DateTime[] dates)
        {
            string[] parts = command.Split(' ', 3);
            int index = int.Parse(parts[1]) - 1;
            
            todos[index] = parts[2];
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача под номером {index + 1} была обновлена.");
        }

        private static void DoneCommand(string command, bool[] statuses, DateTime[] dates)
        {
            int index = int.Parse(command.Split(' ')[1]) - 1;
            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача под номером {index + 1} отмечена выполненной.");
        }

        private static void ViewCommand(string[] todos, bool[] statuses, DateTime[] dates)
        {
            Console.WriteLine("Список задач:");
            for (int i = 0; i < todos.Length; i++)
            {
                if (string.IsNullOrEmpty(todos[i])) continue;
                Console.WriteLine($"{i + 1}) {todos[i]}, сделано:{statuses[i]}, {dates[i]}");
            }
        }

        private static void AddCommand(string command, ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int index)
        {
            string text = command.Substring(4);
            if (index == todos.Length)
            {
                int newSize = todos.Length * 2;
                Array.Resize(ref todos, newSize);
                Array.Resize(ref statuses, newSize);
                Array.Resize(ref dates, newSize);
            }

            todos[index] = text;
            statuses[index] = false;
            dates[index] = DateTime.Now;
            index++;

            Console.WriteLine($"Добавлена задача: \"{text}\"");
        }

        private static void ProfileCommand()
        {
            Console.WriteLine($"{name} {surname}, {age}");
        }

        private static void HelpCommand()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add текст задачи - добавить новую задачу");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("exit - выйти из программы");
        }

        private static void CreateUser()
        {
            Console.Write("Введите ваше имя: ");
            name = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            surname = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            var year = int.Parse(Console.ReadLine());
            age = DateTime.Now.Year - year;
            
            Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
        }
    }
}
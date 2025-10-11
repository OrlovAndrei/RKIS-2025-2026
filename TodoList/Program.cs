namespace TodoList
{
    class Program
    {
        static string name;
        static string surname;
        static int age;

        static string[] taskList = new string[2];
        static bool[] taskStatuses = new bool[2];
        static DateTime[] taskDates = new DateTime[2];
        static int taskCount = 0;
        public static void Main()
        {
            Console.WriteLine("Работу выполнил Кулаков");
            Console.Write("Введите имя: "); 
            name = Console.ReadLine();
            Console.Write("Введите фамилию: ");
            surname = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            int year = int.Parse(Console.ReadLine());
            age = DateTime.Now.Year - year;

            Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
            
            while(true)
            {
                Console.WriteLine("Введите команду: ");
                string command = Console.ReadLine();

                if (command == "help")
                {
                    Help();
                }
                else if (command == "profile")
                {
                    Profile();
                }
                else if (command.StartsWith("add "))
                {
                    AddTask(command);
                }
                else if (command == "view")
                {
                    ViewTasks();
                }
                else if (command == "exit")
                {
                    Console.WriteLine("Программа завершена.");
                    break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
                }
            }
        }

        private static void ViewTasks()
        {
            Console.WriteLine("Список задач:");
            for (var i = 0; i < taskCount; i++)
            {
                Console.WriteLine($"{i + 1}. {taskList[i]} статус:{taskStatuses[i]} {taskDates[i]}");
            }
        }

        static void AddTask(string input)
        {
            string task = input.Split(" ", 2)[1];
            if (taskCount == taskList.Length)
            {
                ExpandArrays();
            }

            taskList[taskCount] = task;
            taskStatuses[taskCount] = false;
            taskDates[taskCount] = DateTime.Now;

            taskCount = taskCount + 1;
            Console.WriteLine($"Задача добавлена: {task}");
        }

        static void Profile()
        {
            Console.WriteLine($"{name} {surname}, {age}");
        }

        static void Help()
        {
            Console.WriteLine("""
            Доступные команды:
            help — список команд
            profile — выводит данные профиля
            add "текст задачи" — добавляет задачу
            view — просмотр всех задач
            exit — завершить программу
            """);
        }
        
        static void ExpandArrays()
        {
            var newSize = taskList.Length * 2;
            Array.Resize(ref taskList, newSize);
            Array.Resize(ref taskStatuses, newSize);
            Array.Resize(ref taskDates, newSize);
        }
    }
}
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
            string name = Console.ReadLine();
            Console.Write("Введите фамилию: ");
            string surname = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            int year = int.Parse(Console.ReadLine());
            int age = DateTime.Now.Year - year;

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
                    string task = command.Split(" ", 2)[1];
                    if (taskCount == taskList.Length)
                    {
                        string[] newTaskList = new string[taskList.Length * 2];
                        for (int i = 0; i < taskList.Length; i++)
                        {
                            newTaskList[i] = taskList[i];
                        }
                        taskList = newTaskList;
                    }

                    taskList[taskCount] = task;
                    taskCount = taskCount + 1;
                    Console.WriteLine($"Задача добавлена: {task}");
                }
                else if (command == "view")
                {
                    Console.WriteLine("Список задач:");
                    foreach (var task in taskList)
                    {
                        if (!string.IsNullOrWhiteSpace(task))
                        {
                            Console.WriteLine(task);
                        }
                    }
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
    }
}
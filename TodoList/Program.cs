using System.Threading.Tasks;

namespace TodoList
{
    class Program
    {
        static string userFirstName;
        static string userLastName;
        static int userBirthYear;

        static string[] tasks = new string[2];
        static bool[] statuses = new bool[2];
        static DateTime[] dates = new DateTime[2];
        static int taskCount = 0;

        public static void Main()
        {
            Console.WriteLine("Работу выполнели Леошко и Петренко 3833");
            Console.Write("Введите ваше имя: ");
            userFirstName = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            userLastName = Console.ReadLine();
            Console.Write("Введите ваш год рождения: ");
            userBirthYear = int.Parse(Console.ReadLine());
            Console.WriteLine($"Добавлен пользователь {userFirstName} {userLastName}, возраст - {DateTime.Now.Year - userBirthYear}");

            while (true)
            {
                Console.Write("\nВведите команду: ");
                var command = Console.ReadLine();

                if (command == "help") ShowHelp();
                else if (command == "profile") ShowProfile();
                else if (command.StartsWith("add ")) AddTask(command);
                else if (command == "view") ViewTasks();
                else if (command.StartsWith("done ")) DoneTask(command);
                else if (command.StartsWith("delete ")) DeleteTask(command);
                else if (command.StartsWith("update ")) UpdateTask(command);
                else if (command == "exit")
                {
                    Console.WriteLine("Программа завершена.");
                    break;
                }
                else Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
            }
        }

        private static void UpdateTask(string command)
        {
            throw new NotImplementedException();
        }

        private static void DeleteTask(string command)
        {
            throw new NotImplementedException();
        }

        private static void ViewTasks()
        {
            Console.WriteLine("Список задач:");
            for (int i = 0; i < taskCount; i++)
                if (!string.IsNullOrEmpty(tasks[i]))
                    Console.WriteLine($"{i} {tasks[i]} {(statuses[i] ? "сделано" : "не сделано")} {dates[i]}");
        }

        private static void DoneTask(string command)
        {
            throw new NotImplementedException();
        }

        private static void AddTask(string command)
        {
            string text = command.Split("add ", 2)[1];
            if (taskCount == tasks.Length)
            {
                string[] newTasks = new string[taskCount * 2];
                bool[] newStatuses = new bool[taskCount * 2];
                DateTime[] newDates = new DateTime[taskCount * 2];
                for (int i = 0; i < tasks.Length; i++)
                {
                    newTasks[i] = tasks[i];
                    newStatuses[i] = statuses[i];
                    newDates[i] = dates[i];
                }

                tasks = newTasks;
                statuses = newStatuses;
                dates = newDates;
            }

            tasks[taskCount] = text;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;
            Console.WriteLine($"Задача добавлена: {text}");
        }

        private static void ShowProfile()
        {
            Console.WriteLine($"{userFirstName} {userLastName}, {userBirthYear}");
        }

        private static void ShowHelp()
        {
            Console.WriteLine("""
            Доступные команды:
            help — список команд
            profile — выводит данные профиля
            add "текст задачи" — добавляет задачу
            view — просмотр всех задач
            done <индекс> — отметить задачу выполненной
            delete <индекс> — удалить задачу
            update <индекс> "новый текст" — изменить текст задачи
            exit — завершить программу
            """);
        }
    }
}
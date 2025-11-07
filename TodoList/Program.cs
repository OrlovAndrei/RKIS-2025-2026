namespace TodoList
{
    class Program
    {
        static string userFirstName;
        static string userLastName;
        static int userBirthYear;

        public static void Main()
        {
	        Console.WriteLine("Работу выполнили Поплевин и Музыка 3831");
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
            throw new NotImplementedException();
        }

        private static void DoneTask(string command)
        {
            throw new NotImplementedException();
        }

        private static void AddTask(string command)
        {
            throw new NotImplementedException();
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
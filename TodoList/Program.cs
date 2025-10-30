using System;

namespace TodoList
{
    class Program
    {
        private static Profile user;
        private static TodoList todoList = new TodoList();

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Морозов и Прокопенко");
            SetUserData();

            while (true)
            {
                Console.Write("Введите команду: ");
                string fullInput = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(fullInput))
                    continue;

                if (fullInput.ToLower() == "exit")
                    return;

                ICommand command = CommandParser.Parse(fullInput, todoList, user);
                command?.Execute();
            }
        }

        static void SetUserData()
        {
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите год вашего рождения: ");
            if (int.TryParse(Console.ReadLine(), out int birthYear))
            {
                user = new Profile(name, lastName, birthYear);
                Console.WriteLine($"Добавлен пользователь: {user.GetInfo()}");
            }
            else
            {
                Console.WriteLine("Ошибка: год рождения должен быть числом");
                SetUserData();
            }
            Console.WriteLine();
        }
    }
}
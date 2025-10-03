using System;

namespace Todolist
{
    class Program
    {
        // Массив в 2 элемента
        static string[] todos = new string[2];
        static string firstName = "";
        static string lastName = "";
        static int birthYear = 0;

        static void Main()
        {
            bool isValid = true;
            int currentYear = DateTime.Now.Year;

            Console.Write("Работу сделали Приходько и Бочкарёв\n");
            Console.Write("Введите свое имя: ");
            string? name = Console.ReadLine();
            Console.Write("Введите свою фамилию: ");
            string? secondName = Console.ReadLine();
            Console.Write("Введите свой год рождения: ");
            int birthYear = 0;

            try
            {
                birthYear = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                isValid = false;
            }


            if ((isValid == true) && (birthYear <= currentYear))
            {
                int age = currentYear - birthYear;
                Console.WriteLine($"Добавлен пользователь:{name},{secondName},возраст - {age}");
            }
            else
            {
                Console.WriteLine("Неверно введен год рождения"); // это ошибка, которая выводится при неправильном введении года рождения, например если введут "арбуз" или введут год больше 2025
            }
            //lesson 3 started there
            Console.WriteLine("Добро пожаловать в программу");
            Console.WriteLine("Введите 'help' для списка команд");
            while (true)
            {
                Console.WriteLine("=-=-=-=-=-=-=-=");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string[] parts = input.Split(' ');
                string command = parts[0].ToLower();

                switch (command)
                {
                    case "help":
                        ShowHelp();
                        break;
                    case "profile":
                        ShowProfile(name, secondName, birthYear);
                        break;
                    case "exit":
                        Console.WriteLine("Выход из программы");
                        return;
                    default:
                        Console.WriteLine($"Неизвестная команда: {command}");
                        break;
                }
            }
            static void ShowHelp()
            {
                Console.WriteLine("Доступные команды");
                Console.WriteLine("help - вывести список команд");
                Console.WriteLine("profile - показать данные пользователя");
                Console.WriteLine("exit - выход из программы");
            }
            static void ShowProfile(string name, string secondName, int birthYear)
            {
                Console.WriteLine($"{name} {secondName} {birthYear}");
            }
        }
    }
}
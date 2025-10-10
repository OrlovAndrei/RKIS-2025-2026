using System;

namespace Todolist
{
    class Program
    {
        // Массив в 2 элемента
        static string[] todos = new string[2]; // задачи
        static bool[] statuses = new bool[2]; // true - выполнено, false - не выполнено
        static DateTime[] dates = new DateTime[2]; // даты
        static string firstName = "";
        static string lastName = "";
        static int birthYear = 0;
        static int todoCount = 0; // 

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
            //ниже код с третьей практической работы

            // инициализация массива задач
            string[] todos = new string[2];
            int todoCount = 0;

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
                    case "add":
                        if(parts.Length < 2)
                        {
                            Console.WriteLine("Ошибка: не указана задача");
                        }
                        else
                        {
                            string task = string.Join(" ", parts, 1, parts.Length - 1);
                            AddTodo(ref todos, ref todoCount, task);
                        }
                        break;
                    case "view":
                        ViewTodos(todos, todoCount);
                        break;
                    case "exit":
                                Console.WriteLine("Выход из программы");
                                return;
                            default:
                                Console.WriteLine($"Неизвестная команда: {command}");
                                break;
                            }
            }
        }
        static void ShowHelp()
            {
                Console.WriteLine("Доступные команды");
                Console.WriteLine("help - вывести список команд");
                Console.WriteLine("profile - показать данные пользователя");
                Console.WriteLine("add - добавить задачу");
                Console.WriteLine("view - показать задачи");
                Console.WriteLine("exit - выход из программы");
            }
        static void ShowProfile(string name, string secondName, int birthYear)
        {
            Console.WriteLine($"{name} {secondName} {birthYear}");
        }
        static void AddTodo(ref string[] todos, ref int todoCount, string task)
        {
            //проверка, нужно ли расширять массив
            if (todoCount >= todos.Length)
            {
            ExpandArrays();
            Console.WriteLine($"Массив расширен до {todos.Length} элементов");
            }

            //здесь добавляется задача и вся информация по ней
            todos[todoCount] = task;
            statuses[todoCount] = false; // по умолчанию задача не выполнена
            dates[todoCount] = DateTime.Now;
            todoCount++;

            Console.WriteLine("Задача добавлена");
            }
        static void ViewTodos(string[] todos, int todoCount)
        {
            if (todoCount == 0)
            {
                Console.WriteLine("Список пуст");
                return;
            }
            Console.WriteLine("Список задач:");
            for (int i = 0; i < todoCount; i++)
            {
                Console.WriteLine($"{i + 1}. {todos[i]}");
            }
        }

        static void CompleteTodo(string numberStr)
        {
            if (int.TryParse(numberStr, out int number) && number > 0 && number <= todoCount)
            {
                int index = number - 1;
                statuses[index] = true;
                dates[index] = DateTime.Now; // обновляем дату при изменении статуса
                Console.WriteLine($"Задача '{todos[index]}' отмечена как выполненная");
            }
            else
            {
                Console.WriteLine("Неверный номер задачи");
            }
        }

        static void ExpandArrays()
        {
            int newSize = todos.Length * 2;

            // Расширяем массив todos
            string[] newTodos = new string[newSize];
            Array.Copy(todos, newTodos, todos.Length);
            todos = newTodos;

            // Расширяем массив statuses
            bool[] newStatuses = new bool[newSize];
            Array.Copy(statuses, newStatuses, statuses.Length);
            statuses = newStatuses;

            // Расширяем массив dates
            DateTime[] newDates = new DateTime[newSize];
            Array.Copy(dates, newDates, dates.Length);
            dates = newDates;

            Console.WriteLine($"Массивы расширены до {newSize} элементов");
        }
    }
}
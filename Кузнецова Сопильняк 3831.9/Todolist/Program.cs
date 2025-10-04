// See https://aka.ms/new-console-template for more information
using System;

namespace TodoList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Powered by Yarr and Angelina");
             // Запрос данных у пользователя
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        string yearInput = Console.ReadLine();

        // Преобразование года рождения в число
        int birthYear;
        while (!int.TryParse(yearInput, out birthYear))
        {
            Console.Write("Ошибка! Введите корректный год рождения: ");
            yearInput = Console.ReadLine();
        }

        // Вывод результата
        Console.WriteLine($"Добро пожаловать пользователь {firstName} {lastName}, возраст - {birthYear}");

            // 2. Создаем массив строк todos с начальной длиной 2 элемента
            string[] todos = new string[2];
            int taskCount = 0; // Счетчик реально добавленных задач

            // 3. Бесконечный цикл для обработки команд
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                // Обработка команд
                switch (input.ToLower())
                {
                    case "help":
                        ShowHelp();
                        break;
                    
                    case "profile":
                        ShowProfile(firstName, lastName, birthYear);
                        break;
                    
                    case "view":
                        ViewTasks(todos, taskCount);
                        break;
                    
                    case "exit":
                        Console.WriteLine("Выход из программы...");
                        return;
                    
                    default:
                        if (input.ToLower().StartsWith("add "))
                        {
                            taskCount = AddTask(input, ref todos, taskCount);
                        }
                        else
                        {
                            Console.WriteLine("Неизвестная команда. Введите 'help' для просмотра доступных команд.");
                        }
                        break;
                }
            }
        }

        // Команда help - вывод списка команд
        static void ShowHelp()
        {
            Console.WriteLine("\n=== Доступные команды ===");
            Console.WriteLine("help    - показать этот список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add     - добавить новую задачу (формат: add \"текст задачи\")");
            Console.WriteLine("view    - показать все задачи");
            Console.WriteLine("exit    - выйти из программы\n");
        }

        // Команда profile - вывод данных пользователя
        static void ShowProfile(string firstName, string lastName, int birthYear)
        {
            Console.WriteLine($"\n=== Профиль пользователя ===");
            Console.WriteLine($"{firstName} {lastName}, {birthYear}\n");
        }

        // Команда add - добавление новой задачи
        static int AddTask(string input, ref string[] todos, int taskCount)
        {
            // Извлекаем текст задачи из команды
            string[] parts = input.Split('"');
            
            if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
            {
                Console.WriteLine("Ошибка: Неверный формат команды. Используйте: add \"текст задачи\"");
                return taskCount;
            }

            string task = parts[1].Trim();
            
            if (string.IsNullOrWhiteSpace(task))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return taskCount;
            }

            // 5. Проверка необходимости расширения массива
            if (taskCount >= todos.Length)
            {
                ExpandArray(ref todos);
                Console.WriteLine($"Массив расширен до {todos.Length} элементов");
            }

            // Добавляем задачу
            todos[taskCount] = task;
            taskCount++;
            Console.WriteLine($"Задача добавлена: \"{task}\"");
            Console.WriteLine($"Всего задач: {taskCount}/{todos.Length}");

            return taskCount;
        }

        // 5. Расширение массива в 2 раза
        static void ExpandArray(ref string[] todos)
        {
            int newSize = todos.Length * 2;
            string[] newArray = new string[newSize];
            
            // Копируем элементы в новый массив
            for (int i = 0; i < todos.Length; i++)
            {
                newArray[i] = todos[i];
            }
            
            todos = newArray;
        }

        // Команда view - просмотр задач
        static void ViewTasks(string[] todos, int taskCount)
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            Console.WriteLine("\n=== Список задач ===");
            for (int i = 0; i < taskCount; i++)
            {
                if (!string.IsNullOrEmpty(todos[i]))
                {
                    Console.WriteLine($"{i + 1}. {todos[i]}");
                }
            }
            Console.WriteLine($"Всего задач: {taskCount}\n");
        }
    }
}


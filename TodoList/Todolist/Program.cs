using System;

namespace Todolist
{
    class Program
    {
        static void Main()
        {
            // Данные пользователя
            Console.WriteLine("Работу выполнили Гергель и Дайнеко 3832");
            Console.WriteLine();

            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            int birthYear = int.Parse(Console.ReadLine());

            int age = DateTime.Now.Year - birthYear;
            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст = {age}");
            Console.WriteLine();

            // Массив для задач (начальный размер - 2 элемента)
            string[] todos = new string[2];
            int taskCount = 0;

            // Бесконечный цикл для команд
            while (true)
            {
                Console.Write("Введите команду (help - список команд): ");
                string input = Console.ReadLine();

                if (input == "help")
                {
                    Console.WriteLine("Доступные команды:");
                    Console.WriteLine("help - вывести список команд");
                    Console.WriteLine("profile - показать данные пользователя");
                    Console.WriteLine("add \"текст задачи\" - добавить задачу");
                    Console.WriteLine("view - показать все задачи");
                    Console.WriteLine("exit - выйти из программы");
                }
                else if (input == "profile")
                {
                    Console.WriteLine($"{firstName} {lastName}, {birthYear} год рождения");
                }
                else if (input.StartsWith("add "))
                {
                    // Извлекаем текст задачи из команды
                    string[] parts = input.Split('"');
                    if (parts.Length >= 2)
                    {
                        string task = parts[1];

                        // Проверяем, нужно ли расширять массив
                        if (taskCount >= todos.Length)
                        {
                            // Создаем новый массив в 2 раза больше
                            string[] newTodos = new string[todos.Length * 2];
                            for (int i = 0; i < todos.Length; i++)
                            {
                                newTodos[i] = todos[i];
                            }
                            todos = newTodos;
                            Console.WriteLine($"Массив расширен до {todos.Length} элементов");
                        }

                        // Добавляем задачу
                        todos[taskCount] = task;
                        taskCount++;
                        Console.WriteLine($"Задача добавлена: {task}");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: используйте формат add \"текст задачи\"");
                    }
                }
                else if (input == "view")
                {
                    Console.WriteLine("Список задач:");
                    bool hasTasks = false;
                    for (int i = 0; i < taskCount; i++)
                    {
                        if (!string.IsNullOrEmpty(todos[i]))
                        {
                            Console.WriteLine($"{i + 1}. {todos[i]}");
                            hasTasks = true;
                        }
                    }

                    if (!hasTasks)
                    {
                        Console.WriteLine("Задач нет");
                    }
                }
                else if (input == "exit")
                {
                    Console.WriteLine("Выход из программы...");
                    break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                }

                Console.WriteLine();
            }
        }
    }
}
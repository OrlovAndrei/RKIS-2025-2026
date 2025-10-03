using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введите имя: ");
        string name = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string surname = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        string birthYearString = Console.ReadLine();

        int birthYear = int.Parse(birthYearString);
        int currentYear = DateTime.Now.Year;
        int age = currentYear - birthYear;

        Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");

        string[] todos = new string[2];
        int todoCount = 0; 

        Console.WriteLine("Введите команду (help - список команд):");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();

            if (input == "help")
            {
                Console.WriteLine("Доступные команды:");
                Console.WriteLine("help - показать список команд");
                Console.WriteLine("profile - показать данные пользователя");
                Console.WriteLine("add \"текст задачи\" - добавить задачу");
                Console.WriteLine("view - показать все задачи");
                Console.WriteLine("exit - выйти из программы");
                continue;
            }

            if (input == "profile")
            {
                Console.WriteLine($"{name} {surname}, возраст - {age}");
                continue;
            }
           
            if (input.StartsWith("add "))
            {
                string[] parts = input.Split(' ');
                if (parts.Length >= 2 && parts[0] == "add")
                {
                    string taskPart = string.Join(" ", parts.Skip(1));
                    if (taskPart.StartsWith("\"") && taskPart.EndsWith("\"") && taskPart.Length > 2)
                    {
                        string task = taskPart.Substring(1, taskPart.Length - 2);

                        if (todoCount == todos.Length)
                        {
                            string[] newTodos = new string[todos.Length * 2];
                            for (int i = 0; i < todos.Length; i++)
                            {
                                newTodos[i] = todos[i];
                            }
                            todos = newTodos;
                        }

                        todos[todoCount] = task;
                        todoCount++;

                        Console.WriteLine("Задача добавлена.");
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\"");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\"");
                }
                continue;
            }

            if (input == "viw")
            {
                if (todoCount == 0)
                {
                    Console.WriteLine("Список задач пуст.");
                }
                else
                {
                    Console.WriteLine("Список задач:");
                    for (int i = 0; i < todoCount; i++)
                    {
                        if (!string.IsNullOrEmpty(todos[i]))
                            Console.WriteLine($"{i + 1}. {todos[i]}");
                    }
                }
                continue;
            }

            if (input == "exit")
            {
                Console.WriteLine("Выход из программы.");
                break;
            }

            Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
        }
    }
}

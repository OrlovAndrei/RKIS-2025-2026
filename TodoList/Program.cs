using System;

class Program
{
    static void Main()
    {
       
        string[] todos = new string[2];
        int todoCount = 0;

       
        Console.WriteLine("Введите имя:");
        string firstName = Console.ReadLine();

        Console.WriteLine("Введите фамилию:");
        string lastName = Console.ReadLine();

        Console.WriteLine("Введите год рождения:");
        int birthYear = int.Parse(Console.ReadLine());

        int currentYear = DateTime.Now.Year;
        int age = currentYear - birthYear;

        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
        Console.WriteLine("Работу выполнил Рублёв");

        
        while (true)
        {
            Console.Write("> ");
            string command = Console.ReadLine();

            if (command == "help")
            {
                Console.WriteLine("Доступные команды:");
                Console.WriteLine("help — список команд");
                Console.WriteLine("profile — показать профиль пользователя");
                Console.WriteLine("add \"текст задачи\" — добавить задачу");
                Console.WriteLine("view — показать все задачи");
                Console.WriteLine("exit — выход из программы");
            }
            else if (command == "profile")
            {
                Console.WriteLine($"{firstName} {lastName}, {birthYear}");
            }
            else if (command.StartsWith("add "))
            {
                string[] parts = command.Split('"');
                if (parts.Length >= 2)
                {
                    string newTask = parts[1];

                   
                    if (todoCount >= todos.Length)
                    {
                        string[] newTodos = new string[todos.Length * 2];
                        for (int i = 0; i < todos.Length; i++)
                            newTodos[i] = todos[i];
                        todos = newTodos;
                    }

                    todos[todoCount] = newTask;
                    todoCount++;
                    Console.WriteLine($"Задача добавлена: \"{newTask}\"");
                }
                else
                {
                    Console.WriteLine("Ошибка: используйте формат add \"текст задачи\"");
                }
            }
            else if (command == "view")
            {
                if (todoCount == 0)
                {
                    Console.WriteLine("Список задач пуст.");
                }
                else
                {
                    Console.WriteLine("Ваши задачи:");
                    for (int i = 0; i < todoCount; i++)
                        Console.WriteLine($"[{i + 1}] {todos[i]}");
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
}







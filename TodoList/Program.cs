using System;

class Program
{
    static void Main(string[] args)
    {
        string name = "Алексей и Лев";
        string surname = "Прокопенко и Морозов";
        int birthYear = 2007;

        string[] todos = new string[2];
        int todoCount = 0; 

        Console.WriteLine("Добро пожаловать, введите help:");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            
          if (input == "help")
            {
                Console.WriteLine("Доступные команды:");
                Console.WriteLine("help - показать список команд");
                Console.WriteLine("profile - показать наши данные");
                Console.WriteLine("add "текст задачи" - добавить задачу");
                Console.WriteLine("view - показать все задания");
                Console.WriteLine("exit - выйти из программы");
                continue;
            }
            if (input == "profile")
            {
                Console.WriteLine($"{name} {surname}, {birthYear}");
                continue;
            }

            if (input.StartsWith("add "))
            {
                string[] parts = input.Split(' ');
                if (parts.Length >= 2 && parts[0] == "add")
                {
                    string taskPart = string.Join(" ", parts.Skip(1));
                    if (taskPart.StartsWith(""") && taskPart.EndsWith(""") && taskPart.Length > 2)
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
                       Console.WriteLine("Неверный формат команды. Используйте: add "текст задачи"");
                    }
                    continue;
                }
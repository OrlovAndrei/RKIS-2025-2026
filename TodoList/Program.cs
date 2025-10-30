using System;

namespace TodoList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Шелепов и Кузьменко");

            Console.WriteLine("Введите имя:");
            string firstName = Console.ReadLine() ?? "";

            Console.WriteLine("Введите фамилию:");
            string lastName = Console.ReadLine() ?? "";

            Console.WriteLine("Введите год рождения:");
            string input = Console.ReadLine() ?? "";

            if (!int.TryParse(input, out int birthYear))
            {
                Console.WriteLine("Ошибка: введите корректный год рождения");
                return;
            }

            var profile = new Profile(firstName, lastName, birthYear);
            var todoList = new TodoList();

            Console.WriteLine($"Профиль создан: {profile.FirstName} {profile.LastName}, возраст {profile.GetAge()}");

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null || line.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                var (command, flags, argsLine) = ParseCommand(line);
                if (string.IsNullOrWhiteSpace(command)) continue;

                switch (command)
                {
                    case "add":
                        todoList.AddTask(argsLine, flags);
                        break;
                    case "done":
                        todoList.MarkTaskDone(argsLine);
                        break;
                    case "delete":
                        todoList.DeleteTask(argsLine);
                        break;
                    case "update":
                        todoList.UpdateTask(argsLine);
                        break;
                    case "view":
                        todoList.ViewTasks(flags);
                        break;
                    case "read":
                        todoList.ReadTask(argsLine);
                        break;
                    case "profile":
                        profile.ShowProfile();
                        break;
                    case "help":
                        ShowHelp();
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                        break;
                }
            }
        }

        static (string command, string[] flags, string argsLine) ParseCommand(string line)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return ("", Array.Empty<string>(), "");

            string command = parts[0].ToLowerInvariant();
            var flagsList = new System.Collections.Generic.List<string>();
            int i = 1;

            for (; i < parts.Length; i++)
            {
                string token = parts[i];
                if (token.StartsWith("--"))
                    flagsList.Add(token.Substring(2));
                else if (token.StartsWith("-") && token.Length > 1)
                {
                    foreach (char c in token.Substring(1))
                    {
                        switch (c)
                        {
                            case 'm': flagsList.Add("multiline"); break;
                            case 'a': flagsList.Add("all"); break;
                            case 'i': flagsList.Add("index"); break;
                            case 's': flagsList.Add("status"); break;
                            case 'd': flagsList.Add("update-date"); break;
                        }
                    }
                }
                else break;
            }

            string argsLine = i < parts.Length ? string.Join(' ', parts, i, parts.Length - i) : "";
            return (command, flagsList.ToArray(), argsLine);
        }

        static void ShowHelp()
        {
            Console.WriteLine("""
            Команды:
            profile                         - выводит данные о пользователе
            add 'текст'                     - добавляет новую задачу (однострочный режим)
            add --multiline  или add -m     - добавляет новую задачу (многострочный режим). Ввод строк до '!end'
            done <номер>                    - пометить задачу как выполненную
            delete <номер>                  - удалить задачу
            update <номер> <текст>          - обновить задачу
            view [флаги]                    - просмотреть список задач (по умолчанию только текст)
                --index, -i       показывать индекс задачи
                --status, -s      показывать статус (выполнена/не выполнена)
                --update-date, -d показывать дату последнего изменения
                --all, -a         показывать все столбцы одновременно
            read <номер>                    - показать полное содержимое задачи
            help                            - показать это сообщение
            exit                            - выйти из программы
            """);
        }
    }
}

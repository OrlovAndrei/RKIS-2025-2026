using System;
using System.Linq;

namespace TodoList
{
    public static class CommandParser
    {
        public static ICommand Parse(string input, TodoList todoList, Profile profile)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            string[] parts = input.Split(' ', 2);
            string command = parts[0].ToLower();
            string arguments = parts.Length > 1 ? parts[1] : "";

            switch (command)
            {
                case "help":
                    return new HelpCommand();

                case "profile":
                    return new ProfileCommand { Profile = profile };

                case "add":
                    return ParseAddCommand(arguments, todoList);

                case "read":
                    return ParseReadCommand(arguments, todoList);

                case "view":
                    return ParseViewCommand(arguments, todoList);

                case "done":
                    return ParseDoneCommand(arguments, todoList);

                case "delete":
                    return ParseDeleteCommand(arguments, todoList);

                case "update":
                    return ParseUpdateCommand(arguments, todoList);

                default:
                    Console.WriteLine($"Неизвестная команда: {command}");
                    Console.WriteLine("Введите 'help' для просмотра доступных команд");
                    return null;
            }
        }

        private static ICommand ParseAddCommand(string arguments, TodoList todoList)
        {
            if (arguments == "--multiline" || arguments == "-m")
            {
                return new AddCommand { IsMultiline = true, TodoList = todoList };
            }
            else if (!string.IsNullOrEmpty(arguments))
            {
                return new AddCommand { IsMultiline = false, TaskText = arguments, TodoList = todoList };
            }
            else
            {
                Console.WriteLine("Неправильный формат: add \"текст задачи\" или add текст задачи");
                return null;
            }
        }

        private static ICommand ParseReadCommand(string arguments, TodoList todoList)
        {
            if (int.TryParse(arguments, out int taskNumber))
            {
                return new ReadCommand { TaskNumber = taskNumber, TodoList = todoList };
            }
            else
            {
                Console.WriteLine("Неверный формат: read номер_задачи");
                return null;
            }
        }

        private static ICommand ParseViewCommand(string arguments, TodoList todoList)
        {
            return new ViewCommand
            {
                ShowIndex = arguments.Contains("-i") || arguments.Contains("--index"),
                ShowStatus = arguments.Contains("-s") || arguments.Contains("--status"),
                ShowDate = arguments.Contains("-d") || arguments.Contains("--update-date"),
                ShowAll = arguments.Contains("-a") || arguments.Contains("--all"),
                TodoList = todoList
            };
        }

        private static ICommand ParseDoneCommand(string arguments, TodoList todoList)
        {
            if (int.TryParse(arguments, out int taskNumber))
            {
                return new DoneCommand { TaskNumber = taskNumber, TodoList = todoList };
            }
            else
            {
                Console.WriteLine("Неправильный формат: done номер_задачи");
                return null;
            }
        }

        private static ICommand ParseDeleteCommand(string arguments, TodoList todoList)
        {
            if (int.TryParse(arguments, out int taskNumber))
            {
                return new DeleteCommand { TaskNumber = taskNumber, TodoList = todoList };
            }
            else
            {
                Console.WriteLine("Неверный формат: delete номер_задачи");
                return null;
            }
        }

        private static ICommand ParseUpdateCommand(string arguments, TodoList todoList)
        {
            string[] parts = arguments.Split(' ', 2);

            if (parts.Length < 2)
            {
                Console.WriteLine("Не указан новый текст задачи");
                return null;
            }

            if (int.TryParse(parts[0], out int taskNumber))
            {
                return new UpdateCommand { TaskNumber = taskNumber, NewText = parts[1], TodoList = todoList };
            }
            else
            {
                Console.WriteLine("Неверный формат: update номер_задачи \"новый текст\"");
                return null;
            }
        }
    }
}
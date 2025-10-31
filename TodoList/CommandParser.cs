using System;

namespace TodoList
{
    public static class CommandParser
    {
        public static ICommand Parse(string input, TodoList todoList, Profile profile)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string cmd = parts[0].ToLower();

            switch (cmd)
            {
                case "add":
                    bool multiline = input.Contains("--multiline") || input.Contains("-m");
                    string text = parts.Length > 1 ? parts[1].Trim() : "";
                    return new AddCommand { TodoList = todoList, Text = text, IsMultiline = multiline };

                case "view":
                    var vc = new ViewCommand { TodoList = todoList };
                    foreach (char c in input)
                    {
                        switch (c)
                        {
                            case 'a': vc.ShowIndex = vc.ShowDone = vc.ShowDate = true; break;
                            case 'i': vc.ShowIndex = true; break;
                            case 's': vc.ShowDone = true; break;
                            case 'd': vc.ShowDate = true; break;
                        }
                    }
                    return vc;

                case "done":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int doneIdx))
                        throw new Exception("Укажите номер задачи");
                    return new DoneCommand { TodoList = todoList, Index = doneIdx - 1 };

                case "delete":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int delIdx))
                        throw new Exception("Укажите номер задачи");
                    return new DeleteCommand { TodoList = todoList, Index = delIdx - 1 };

                case "profile":
                    return new ProfileCommand { Profile = profile };

                case "help":
                    return new HelpCommand();

                case "exit":
                    return new ExitCommand();

                default:
                    Console.WriteLine("Неизвестная команда. Введите help.");
                    return null;
            }
        }
    }
}

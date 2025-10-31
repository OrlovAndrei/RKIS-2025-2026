using System;

namespace TodoList
{
    public static class CommandParser
    {
        public static ICommand? Parse(string inputString, TodoList todoList, Profile profile)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return null;

            var parts = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return null;

            string commandName = parts[0].ToLowerInvariant();
            var flags = new List<string>();
            int i = 1;

            for (; i < parts.Length; i++)
            {
                string token = parts[i];
                if (token.StartsWith("--"))
                    flags.Add(token.Substring(2));
                else if (token.StartsWith("-") && token.Length > 1)
                {
                    foreach (char c in token.Substring(1))
                    {
                        switch (c)
                        {
                            case 'm': flags.Add("multiline"); break;
                            case 'a': flags.Add("all"); break;
                            case 'i': flags.Add("index"); break;
                            case 's': flags.Add("status"); break;
                            case 'd': flags.Add("update-date"); break;
                        }
                    }
                }
                else break;
            }

            string arg = i < parts.Length ? string.Join(' ', parts, i, parts.Length - i) : string.Empty;

            return commandName switch
            {
                "add" => new AddCommand
                {
                    Text = arg,
                    IsMultiline = flags.Contains("multiline"),
                    TodoList = todoList
                },
                "view" => new ViewCommand
                {
                    TodoList = todoList,
                    ShowAll = flags.Contains("all"),
                    ShowIndex = flags.Contains("index"),
                    ShowStatus = flags.Contains("status"),
                    ShowDate = flags.Contains("update-date")
                },
                "done" => new DoneCommand
                {
                    Arg = arg,
                    TodoList = todoList
                },
                "delete" => new DeleteCommand
                {
                    Arg = arg,
                    TodoList = todoList
                },
                "update" => new UpdateCommand
                {
                    Arg = arg,
                    TodoList = todoList
                },
                "read" => new ReadCommand
                {
                    Arg = arg,
                    TodoList = todoList
                },
                "profile" => new ProfileCommand
                {
                    Profile = profile
                },
                _ => null
            };
        }
    }
}
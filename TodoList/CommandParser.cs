namespace TodoList;

public static class CommandParser
{
    public static ICommand? Parse(string input, TodoList todoList, Profile profile, Action exitAction)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string commandName = parts[0].ToLower();

        var flags = ParseFlags(parts);

        switch (commandName)
        {
            case "help":
                return new HelpCommand();

            case "profile":
                return new ProfileCommand(profile);

            case "add":
                string text = ExtractText(parts);
                bool isMultiline = flags.Contains("--multiline") || flags.Contains("-m");
                return new AddCommand(todoList, text, isMultiline);

            case "view":
                bool showAll = flags.Contains("--all") || flags.Contains("-a");
                bool showIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
                bool showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
                bool showUpdateDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;
                return new ViewCommand(todoList, showIndex, showStatus, showUpdateDate);

            case "done":
                if (parts.Length < 2)
                {
                    Console.WriteLine("Ошибка: укажите индекс задачи.");
                    return null;
                }
                if (!int.TryParse(parts[1], out int doneIndex))
                {
                    Console.WriteLine("Ошибка: индекс должен быть числом.");
                    return null;
                }
                return new DoneCommand(todoList, doneIndex - 1); 

            case "delete":
                if (parts.Length < 2)
                {
                    Console.WriteLine("Ошибка: укажите индекс задачи.");
                    return null;
                }
                if (!int.TryParse(parts[1], out int deleteIndex))
                {
                    Console.WriteLine("Ошибка: индекс должен быть числом.");
                    return null;
                }
                return new DeleteCommand(todoList, deleteIndex - 1);

            case "update":
                if (parts.Length < 3)
                {
                    Console.WriteLine("Ошибка: укажите индекс и новый текст задачи.");
                    return null;
                }
                if (!int.TryParse(parts[1], out int updateIndex))
                {
                    Console.WriteLine("Ошибка: индекс должен быть числом.");
                    return null;
                }
                string newText = string.Join(" ", parts.Skip(2));
                return new UpdateCommand(todoList, updateIndex - 1, newText);

            case "read":
                if (parts.Length < 2)
                {
                    Console.WriteLine("Ошибка: укажите индекс задачи.");
                    return null;
                }
                if (!int.TryParse(parts[1], out int readIndex))
                {
                    Console.WriteLine("Ошибка: индекс должен быть числом.");
                    return null;
                }
                return new ReadCommand(todoList, readIndex - 1);

            case "exit":
                return new ExitCommand(exitAction);

            default:
                Console.WriteLine("Неизвестная команда");
                return null;
        }
    }

    private static string[] ParseFlags(string[] parts)
    {
        var flags = new List<string>();
        foreach (var part in parts)
        {
            if (part.StartsWith("--"))
                flags.Add(part);
            else if (part.StartsWith("-") && part.Length > 1)
            {
                for (int i = 1; i < part.Length; i++)
                    flags.Add("-" + part[i]);
            }
        }
        return flags.ToArray();
    }

    private static string ExtractText(string[] parts)
    {
        var textParts = parts.Skip(1).Where(p => !p.StartsWith("-")).ToArray();
        return string.Join(" ", textParts);
    }
}
using System;

static class CommandParser
{
    public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            return null;
        }

        string[] parts = inputString.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string command = parts.Length > 0 ? parts[0].ToLower() : string.Empty;
        string args = parts.Length > 1 ? parts[1] : string.Empty;

        switch (command)
        {
            case "profile":
                return new ProfileCommand(profile);

            case "add":
                return new AddCommand(todoList, args);

            case "view":
                return new ViewCommand(todoList, args);

            case "read":
                if (TryParseIndex(args, todoList.Count, out int readIndex))
                {
                    return new ReadCommand(todoList, readIndex);
                }
                return null;

            case "done":
                if (TryParseIndex(args, todoList.Count, out int doneIndex))
                {
                    return new DoneCommand(todoList, doneIndex);
                }
                return null;

            case "delete":
                if (TryParseIndex(args, todoList.Count, out int deleteIndex))
                {
                    return new DeleteCommand(todoList, deleteIndex);
                }
                return null;

            case "update":
                if (!string.IsNullOrWhiteSpace(args))
                {
                    string[] updateParts = args.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (updateParts.Length >= 2 && int.TryParse(updateParts[0], out int updateIndex))
                    {
                        string newText = updateParts[1].Trim().Trim('"');
                        return new UpdateCommand(todoList, updateIndex, newText);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный формат. Пример: update 2 \"Новый текст\"");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: укажите индекс и новый текст. Пример: update 2 \"Новый текст\"");
                }
                return null;

            default:
                return null;
        }
    }

    private static bool TryParseIndex(string arg, int taskCount, out int indexOneBased)
    {
        indexOneBased = -1;
        if (string.IsNullOrWhiteSpace(arg))
        {
            Console.WriteLine("Ошибка: укажите индекс задачи.");
            return false;
        }

        if (!int.TryParse(arg.Trim(), out int idxOneBased))
        {
            Console.WriteLine("Ошибка: индекс должен быть числом.");
            return false;
        }

        indexOneBased = idxOneBased;
        if (indexOneBased < 1 || indexOneBased > taskCount)
        {
            Console.WriteLine("Ошибка: индекс вне диапазона.");
            return false;
        }

        return true;
    }
}


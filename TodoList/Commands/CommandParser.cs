using System.Linq;

namespace TodoList
{
    public static class CommandParser
    {
        public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new ArgumentException("Введена пустая строка.");
            }

            string[] parts = inputString.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLower();

            switch (command)
            {
                case "help":
                    return new HelpCommand();

                case "profile":
                    if (profile == null)
                    {
                        throw new ArgumentNullException(nameof(profile), "Profile не может быть null.");
                    }
                    return new ProfileCommand(profile);

                case "exit":
                    return new ExitCommand();

                case "add":
                    if (todoList == null)
                    {
                        throw new ArgumentNullException(nameof(todoList), "TodoList не может быть null.");
                    }
                    if (parts.Length < 2)
                    {
                        throw new ArgumentException("Недостаточно параметров для команды add.");
                    }
                    if (parts[1] == "--multiline" || parts[1] == "-m")
                    {
                        return new AddCommand(todoList, "", true);
                    }
                    else
                    {
                        string text = string.Join(" ", parts.Skip(1));
                        return new AddCommand(todoList, text, false);
                    }

                case "view":
                    if (todoList == null)
                    {
                        throw new ArgumentNullException(nameof(todoList), "TodoList не может быть null.");
                    }
                    bool showIndex = false;
                    bool showStatus = false;
                    bool showDate = false;
                    foreach (string part in parts.Skip(1))
                    {
                        switch (part)
                        {
                            case "--index":
                            case "-i":
                                showIndex = true;
                                break;
                            case "--status":
                            case "-s":
                                showStatus = true;
                                break;
                            case "--update-date":
                            case "-d":
                                showDate = true;
                                break;
                            case "--all":
                            case "-a":
                                showIndex = true;
                                showStatus = true;
                                showDate = true;
                                break;
                        }
                    }
                    return new ViewCommand(todoList, showIndex, showStatus, showDate);

                case "read":
                    if (todoList == null)
                    {
                        throw new ArgumentNullException(nameof(todoList), "TodoList не может быть null.");
                    }
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int readIndex))
                    {
                        throw new ArgumentException("Неверный индекс для команды read.");
                    }
                    return new ReadCommand(todoList, readIndex);

                case "status":
                    if (todoList == null)
                    {
                        throw new ArgumentNullException(nameof(todoList), "TodoList не может быть null.");
                    }
                    if (parts.Length < 3 || !int.TryParse(parts[1], out int statusIndex))
                    {
                        throw new ArgumentException("Неверный индекс или статус для команды status. Пример: status 1 completed");
                    }
                    if (!Enum.TryParse<TodoStatus>(parts[2], true, out TodoStatus status))
                    {
                        throw new ArgumentException("Неверный статус. Доступные: NotStarted, InProgress, Completed, Postponed, Failed");
                    }
                    return new StatusCommand(todoList, statusIndex, status);

                case "delete":
                    if (todoList == null)
                    {
                        throw new ArgumentNullException(nameof(todoList), "TodoList не может быть null.");
                    }
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int deleteIndex))
                    {
                        throw new ArgumentException("Неверный индекс для команды delete.");
                    }
                    return new DeleteCommand(todoList, deleteIndex);

                case "update":
                    if (todoList == null)
                    {
                        throw new ArgumentNullException(nameof(todoList), "TodoList не может быть null.");
                    }
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int updateIndex))
                    {
                        throw new ArgumentException("Неверный индекс для команды update.");
                    }
                    if (parts.Length < 3)
                    {
                        throw new ArgumentException("Недостаточно параметров для команды update.");
                    }
                    if (parts[2] == "--multiline" || parts[2] == "-m")
                    {
                        return new UpdateCommand(todoList, updateIndex, "", true);
                    }
                    else
                    {
                        string updateText = string.Join(" ", parts.Skip(2));
                        return new UpdateCommand(todoList, updateIndex, updateText, false);
                    }

                default:
                    throw new ArgumentException("Неизвестная команда.");
            }
        }
    }
}
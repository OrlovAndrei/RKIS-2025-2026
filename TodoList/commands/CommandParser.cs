namespace TodoList.commands
{
    public static class CommandParser
    {
        public static ICommand? Parse(string input, TodoList todoList, Profile profile)
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

                case "status":
                    var newDoneParts = parts[1].Split();
                    if (newDoneParts.Length < 2 || !Enum.TryParse(newDoneParts[1], ignoreCase: true, out TodoStatus status))
                        throw new Exception("Укажите правильный статус");
                    if (!int.TryParse(newDoneParts[0], out int doneIdx))
                        throw new Exception("Укажите номер задачи");
                    return new StatusCommand { TodoList = todoList, Index = doneIdx - 1, Status = status};

                case "delete":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int delIdx))
                        throw new Exception("Укажите номер задачи");
                    return new DeleteCommand { TodoList = todoList, Index = delIdx - 1 };
                
                case "update":
                    var newParts = parts[1].Split();
                    string updatedText = newParts.Length > 1 ? newParts[1].Trim() : "";
                    if (newParts.Length < 1 || !int.TryParse(newParts[0], out int updateIdx))
                        throw new Exception("Укажите номер задачи");
                    return new UpdateCommand { TodoList = todoList, Index = updateIdx - 1, Text = updatedText };

                case "read":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int readIdx))
                        throw new Exception("Укажите номер задачи");
                    return new ReadCommand { TodoList = todoList, Index = readIdx - 1 };
                
                case "profile":
                    return new ProfileCommand { Profile = profile };

                case "help":
                    return new HelpCommand();

                case "exit":
                    return new ExitCommand{Profile = profile, TodoList = todoList};

                case "undo":
                    return new UndoCommand();
                
                case "redo":
                    return new RedoCommand();
                default:
                    Console.WriteLine("Неизвестная команда. Введите help.");
                    return null;
            }
        }
    }
}

using System;

class CommandParser
{
    private TodoList todoList;
    private Profile profile;

    public CommandParser(TodoList todoList, Profile profile)
    {
        this.todoList = todoList;
        this.profile = profile;
    }

    public ICommand? Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        string[] parts = input.Split(' ', 3);
        string command = parts[0];

        switch (command)
        {
            case "add":
                if (parts.Length < 2) return null;
                return new AddCommand
                {
                    TodoList = todoList,
                    Text = parts[1]
                };

            case "view":
                return new ViewCommand
                {
                    TodoList = todoList,
                    ShowIndex = true,
                    ShowDone = true,
                    ShowDate = true
                };

            case "done":
                if (!TryGetIndex(parts, out int d)) return null;
                return new DoneCommand
                {
                    TodoList = todoList,
                    Index = d
                };

            case "update":
                if (parts.Length < 3) return null;
                if (!TryGetIndex(parts, out int u)) return null;
                return new UpdateCommand
                {
                    TodoList = todoList,
                    Index = u,
                    Text = parts[2]
                };

            case "read":
                if (!TryGetIndex(parts, out int r)) return null;
                return new ReadCommand
                {
                    TodoList = todoList,
                    Index = r
                };

            case "profile":
                return new ProfileCommand
                {
                    Profile = profile
                };

            default:
                return null;
        }
    }

    private bool TryGetIndex(string[] parts, out int index)
    {
        index = -1;
        return parts.Length >= 2 && int.TryParse(parts[1], out index);
    }
}




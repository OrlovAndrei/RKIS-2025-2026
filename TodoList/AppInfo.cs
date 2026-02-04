using TodoList.commands;

namespace TodoList;

public static class AppInfo
{
    public static Stack<ICommand> UndoStack { get; } = new();
    public static Stack<ICommand> RedoStack { get; } = new();
}
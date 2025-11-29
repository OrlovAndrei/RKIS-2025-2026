using System.Collections.Generic;
using Todolist.Commands;

static class AppInfo
{
    public static TodoList Todos { get; set; } = new TodoList();
    public static Profile CurrentProfile { get; set; } = null!;
    public static Stack<ICommand> UndoStack { get; set; } = new Stack<ICommand>();
    public static Stack<ICommand> RedoStack { get; set; } = new Stack<ICommand>();
}


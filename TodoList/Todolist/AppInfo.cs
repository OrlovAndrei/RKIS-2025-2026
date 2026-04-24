using System.Collections.Generic;

namespace Todolist
{
    public static class AppInfo
    {
        public static TodoList Todos { get; set; }
        public static Profile CurrentProfile { get; set; }
        public static Stack<ICommand> UndoStack { get; set; } = new Stack<ICommand>();
        public static Stack<ICommand> RedoStack { get; set; } = new Stack<ICommand>();
    }
}
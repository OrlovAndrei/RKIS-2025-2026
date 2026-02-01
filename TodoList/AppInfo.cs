using System.Collections.Generic;

namespace TodoApp
{
    public static class AppInfo
    {
        // Текущие данные приложения
        public static TodoList Todos { get; set; }
        public static Profile CurrentProfile { get; set; }
        
        // Пути к файлам
        public static string TodosFilePath => System.IO.Path.Combine("data", "todo.csv");
        public static string ProfileFilePath => System.IO.Path.Combine("data", "profile.txt");
        
        // Стеки для undo/redo
        public static Stack<ICommand> UndoStack { get; } = new Stack<ICommand>();
        public static Stack<ICommand> RedoStack { get; } = new Stack<ICommand>();
    }
}
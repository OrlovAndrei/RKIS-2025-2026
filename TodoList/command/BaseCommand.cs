using System;

namespace TodoApp.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        
        public abstract bool Execute();
        public abstract bool Unexecute();
        
        // Вспомогательный метод для сохранения команды в стек undo
        protected void PushToUndoStack()
        {
            AppInfo.UndoStack.Push(this);
            // При выполнении новой команды очищаем стек redo
            AppInfo.RedoStack.Clear();
        }
        
        // Метод для автосохранения
        protected void AutoSave()
        {
            if (AppInfo.Todos != null)
            {
                FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodosFilePath);
            }
        }
    }
}
using System;

namespace Todolist
{
    public class DeleteCommand : ICommand
    {
        public int Index { get; set; }
        private TodoItem _deletedItem;
        private int _deletedIndex;

        public void Execute()
        {
            int taskIndex = Index - 1;
            TodoItem item = AppInfo.Todos.GetItem(taskIndex);
            if (item != null)
            {
                _deletedItem = item;
                _deletedIndex = taskIndex;
                string deletedText = item.Text;
                AppInfo.Todos.Delete(taskIndex);
                Console.WriteLine($"Задача удалена: {deletedText}");

                string dataDir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data");
                string todoFile = System.IO.Path.Combine(dataDir, "todo.csv");
                FileManager.SaveTodos(AppInfo.Todos, todoFile);
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
            }
        }

        public void Unexecute()
        {
            if (_deletedItem != null)
            {
                AppInfo.Todos.Add(_deletedItem);
                Console.WriteLine($"Восстановлена удаленная задача: {_deletedItem.Text}");
            }
        }
    }
}

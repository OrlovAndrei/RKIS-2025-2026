using System;

namespace Todolist
{
    public class UpdateCommand : ICommand
    {
        public int Index { get; set; }
        public string NewText { get; set; }
        private string _oldText;
        private int _taskIndex;

        public void Execute()
        {
            _taskIndex = Index - 1;
            TodoItem item = AppInfo.Todos.GetItem(_taskIndex);
            if (item != null)
            {
                _oldText = item.Text;
                item.UpdateText(NewText);
                Console.WriteLine($"Задача обновлена: '{_oldText}' -> '{NewText}'");

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
            TodoItem item = AppInfo.Todos.GetItem(_taskIndex);
            if (item != null)
            {
                item.UpdateText(_oldText);
                Console.WriteLine($"Отменено обновление: '{NewText}' -> '{_oldText}'");
            }
        }
    }
}

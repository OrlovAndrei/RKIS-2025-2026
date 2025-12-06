using System;

namespace Todolist
{
    public class DeleteCommand : ICommand
    {
        public int TaskNumber { get; private set; }
        private TodoItem _deletedItem;
        private int _deletedIndex;

        public DeleteCommand(int taskNumber)
        {
            TaskNumber = taskNumber;
        }

        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
            {
                Console.WriteLine("Ошибка: необходимо войти в профиль");
                return;
            }

            var todoList = AppInfo.GetCurrentTodos();
            
            if (TaskNumber < 1 || TaskNumber > todoList.GetCount())
            {
                Console.WriteLine($"Ошибка: задача №{TaskNumber} не существует");
                return;
            }

            _deletedIndex = TaskNumber - 1;
            _deletedItem = todoList.GetItem(_deletedIndex);
            string taskText = _deletedItem.Text;
            todoList.Delete(_deletedIndex);
            Console.WriteLine($"Задача №{TaskNumber} '{taskText}' удалена");
            
            FileManager.SaveTodos(todoList, AppInfo.CurrentProfileId.Value);
        }

        public void Unexecute()
        {
            if (!AppInfo.CurrentProfileId.HasValue || _deletedItem == null)
                return;

            var todoList = AppInfo.GetCurrentTodos();
            
            todoList.Insert(_deletedItem, _deletedIndex);
            Console.WriteLine($"Отменено удаление задачи №{TaskNumber}: {_deletedItem.Text}");
            
            FileManager.SaveTodos(todoList, AppInfo.CurrentProfileId.Value);
        }
    }
}
using System;
using Todolist.Exceptions;
using Todolist.Models;

namespace Todolist
{
    public class DeleteCommand : ICommand
    {
        public int TaskNumber { get; private set; }
        private TodoItem? _deletedItem;
        private int _deletedIndex;

        public DeleteCommand(int taskNumber)
        {
            TaskNumber = taskNumber;
        }

        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
                throw new AuthenticationException("Необходимо войти в профиль");

            var todoList = AppInfo.GetCurrentTodos();

            if (TaskNumber < 1 || TaskNumber > todoList.GetCount())
                throw new TaskNotFoundException($"Задача №{TaskNumber} не существует");

            _deletedIndex = TaskNumber - 1;
            _deletedItem = todoList.GetItem(_deletedIndex);
            string taskText = _deletedItem.Text;

            AppInfo.TodoRepository.Delete(_deletedItem.Id);

            todoList.Delete(_deletedIndex);

            Console.WriteLine($"Задача №{TaskNumber} '{taskText}' удалена");
        }

        public void Unexecute()
        {
            if (!AppInfo.CurrentProfileId.HasValue || _deletedItem == null)
                return;

            var todoList = AppInfo.GetCurrentTodos();

            _deletedItem.Id = 0;
            AppInfo.TodoRepository.Add(_deletedItem);

            todoList.Insert(_deletedItem, _deletedIndex);

            Console.WriteLine($"Отменено удаление задачи №{TaskNumber}: {_deletedItem.Text}");
        }
    }
}
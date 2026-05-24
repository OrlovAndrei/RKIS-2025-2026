using System;
using Todolist.Exceptions;
using Todolist.Models;

namespace Todolist
{
    public class UpdateCommand : ICommand
    {
        public int TaskNumber { get; private set; }
        public string NewText { get; private set; }
        private string? _oldText;
        private TodoItem? _updatedItem;

        public UpdateCommand(int taskNumber, string newText)
        {
            TaskNumber = taskNumber;
            NewText = newText;
        }

        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
                throw new AuthenticationException("Необходимо войти в профиль");

            var todoList = AppInfo.GetCurrentTodos();

            if (TaskNumber < 1 || TaskNumber > todoList.GetCount())
                throw new TaskNotFoundException($"Задача №{TaskNumber} не существует");

            _updatedItem = todoList.GetItem(TaskNumber - 1);
            _oldText = _updatedItem.Text;

            if (NewText.StartsWith("\"") && NewText.EndsWith("\""))
            {
                NewText = NewText.Substring(1, NewText.Length - 2);
            }

            _updatedItem.UpdateText(NewText);

            AppInfo.TodoRepository.Update(_updatedItem);

            todoList.Refresh();

            Console.WriteLine($"Обновил задачу: \nБыло: Задача №{TaskNumber} \"{_oldText}\" \nСтало: Задача №{TaskNumber} \"{NewText}\"");
        }

        public void Unexecute()
        {
            if (!AppInfo.CurrentProfileId.HasValue || _updatedItem == null || _oldText == null)
                return;

            var todoList = AppInfo.GetCurrentTodos();

            _updatedItem.UpdateText(_oldText);
            AppInfo.TodoRepository.Update(_updatedItem);
            todoList.Refresh();

            Console.WriteLine($"Отменено обновление задачи №{TaskNumber}. Восстановлен текст: {_oldText}");
        }
    }
}
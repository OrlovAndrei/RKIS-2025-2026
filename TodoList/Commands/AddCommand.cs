using System;
using Todolist.Exceptions;
using Todolist.Models;

namespace Todolist
{
    public class AddCommand : ICommand
    {
        public bool IsMultiline { get; private set; }
        public string TaskText { get; private set; }
        private TodoItem? _addedItem;
        private int _addedIndex;

        public AddCommand(string taskText, bool isMultiline = false)
        {
            TaskText = taskText;
            IsMultiline = isMultiline;
        }

        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
                throw new AuthenticationException("Необходимо войти в профиль");

            var todoList = AppInfo.GetCurrentTodos();

            if (IsMultiline)
            {
                Console.WriteLine("Многострочный режим. Введите задачи (для завершения введите '!end'):");
                string multilineText = "";

                Console.Write("> ");
                string? line = Console.ReadLine();
                while (line != null && line.ToLower() != "!end")
                {
                    multilineText += line + "\n";
                    Console.Write("> ");
                    line = Console.ReadLine();
                }

                TaskText = multilineText.Trim();
            }
            else if (TaskText.StartsWith("\"") && TaskText.EndsWith("\""))
            {
                TaskText = TaskText.Substring(1, TaskText.Length - 2);
            }

            _addedItem = new TodoItem(TaskText)
            {
                ProfileId = AppInfo.CurrentProfileId.Value
            };

            AppInfo.TodoRepository.Add(_addedItem);

            todoList.Add(_addedItem);
            _addedIndex = todoList.GetCount() - 1;

            Console.WriteLine($"Добавлена задача №{_addedIndex + 1}: {TaskText}");
        }

        public void Unexecute()
        {
            if (!AppInfo.CurrentProfileId.HasValue || _addedItem == null)
                return;

            var todoList = AppInfo.GetCurrentTodos();

            AppInfo.TodoRepository.Delete(_addedItem.Id);

            if (_addedIndex >= 0 && _addedIndex < todoList.GetCount())
            {
                todoList.Delete(_addedIndex);
            }

            Console.WriteLine($"Отменено добавление задачи: {TaskText}");
        }
    }
}
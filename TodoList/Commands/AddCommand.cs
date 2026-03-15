using System;
using TodoList.Exceptions;

namespace TodoList.Commands
{
    public class AddCommand : ICommand, IUndo
    {
        public string Text { get; set; }
        public bool IsMultiline { get; set; }
        public string[] Flags { get; set; }

        private TodoItem _addedItem;
        private int _addedIndex;

        public void Execute()
        {
            var todos = AppInfo.CurrentUserTodos;
            if (todos == null)
            {
                throw new AuthenticationException("Не удалось получить список задач. Войдите в профиль.");
            }

            if (IsMultiline)
            {
                todos.AddTask("", Flags ?? Array.Empty<string>());
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Text))
                {
                    throw new InvalidArgumentException("Не введён текст задачи.");
                }
                todos.AddTask(Text, Flags ?? Array.Empty<string>());
            }
            
            _addedIndex = todos.GetAllTasks().Count - 1;
            _addedItem = todos.GetAllTasks()[_addedIndex];

            Program.SaveCurrentUserTasks();
        }

        public void Unexecute()
        {
            var todos = AppInfo.CurrentUserTodos;
            if (_addedItem != null && todos != null)
            {
                var tasks = todos.GetAllTasks();
                tasks.RemoveAt(_addedIndex);
                Console.WriteLine("Добавление задачи отменено");
                Program.SaveCurrentUserTasks();
            }
        }
    }
}
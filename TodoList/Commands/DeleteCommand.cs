using System;

namespace TodoList.Commands
{
    public class DeleteCommand : ICommand, IUndo
    {
        public string Arg { get; set; }

        private TodoItem _deletedItem;
        private int _deletedIndex;

        public void Execute()
        {
            var todos = AppInfo.CurrentUserTodos;
            if (todos == null)
            {
                Console.WriteLine("Ошибка: не удалось получить список задач. Войдите в профиль.");
                return;
            }

            if (!int.TryParse(Arg, out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            _deletedIndex = idx - 1;
            var tasks = todos.GetAllTasks();
            if (_deletedIndex < 0 || _deletedIndex >= tasks.Count)
            {
                Console.WriteLine("Ошибка: некорректный номер задачи");
                return;
            }

            _deletedItem = tasks[_deletedIndex];
            tasks.RemoveAt(_deletedIndex);
            Console.WriteLine("Задача удалена");
            Program.SaveCurrentUserTasks();
        }

        public void Unexecute()
        {
            var todos = AppInfo.CurrentUserTodos;
            if (_deletedItem != null && todos != null)
            {
                var tasks = todos.GetAllTasks();
                tasks.Insert(_deletedIndex, _deletedItem);
                Console.WriteLine("Удаление задачи отменено");
                Program.SaveCurrentUserTasks();
            }
        }
    }
}
using System;
using Todolist.Exceptions;

namespace Todolist.Commands
{
    internal class UpdateCommand : ICommand
    {
        public int Index { get; set; }
        public string NewText { get; set; }
        private string? oldText = null;

        public UpdateCommand(int index, string newText)
        {
            Index = index;
            NewText = newText ?? string.Empty;
        }

        public void Execute()
        {
            if (AppInfo.CurrentProfileId == Guid.Empty)
                throw new AuthenticationException("Необходимо войти в профиль для работы с задачами.");
            TodoItem item = AppInfo.Todos.GetItem(Index);
            oldText = item.Text;
            AppInfo.Todos.Update(Index, NewText);
            Console.WriteLine($"Задача {Index} обновлена.");
        }

        public void Unexecute()
        {
            if (oldText != null && Index >= 1 && Index <= AppInfo.Todos.Count)
            {
                AppInfo.Todos.Update(Index, oldText);
            }
        }
    }
}


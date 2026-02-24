using System;
using Todolist.Exceptions;

namespace Todolist.Commands
{
    internal class DeleteCommand : ICommand
    {
        public int Index { get; set; }
        private TodoItem? deletedItem = null;
        private int? deletedIndex = null;

        public DeleteCommand(int index)
        {
            Index = index;
        }

        public void Execute()
        {
            if (AppInfo.CurrentProfileId == Guid.Empty)
                throw new AuthenticationException("Необходимо войти в профиль для работы с задачами.");
            deletedItem = AppInfo.Todos.GetItem(Index);
            deletedIndex = Index;
            TodoItem copy = new TodoItem(deletedItem.Text);
            copy.Status = deletedItem.Status;
            copy.LastUpdate = deletedItem.LastUpdate;
            deletedItem = copy;
            AppInfo.Todos.Delete(Index);
            Console.WriteLine($"Удалена задача {Index}.");
        }

        public void Unexecute()
        {
            if (deletedItem != null && deletedIndex.HasValue)
            {
                AppInfo.Todos.Insert(deletedIndex.Value, deletedItem);
            }
        }
    }
}


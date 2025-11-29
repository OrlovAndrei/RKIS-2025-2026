using System;

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
            try
            {
                if (Index < 1 || Index > AppInfo.Todos.Count)
                {
                    Console.WriteLine("Ошибка: индекс вне диапазона.");
                    return;
                }

                deletedItem = AppInfo.Todos.GetItem(Index);
                deletedIndex = Index;
                
                // Создаем копию элемента для отмены
                TodoItem copy = new TodoItem(deletedItem.Text);
                copy.Status = deletedItem.Status;
                copy.LastUpdate = deletedItem.LastUpdate;
                deletedItem = copy;

                AppInfo.Todos.Delete(Index);
                FileManager.SaveTodos(AppInfo.Todos, Program.TodoFilePath);
                Console.WriteLine($"Задача {Index} удалена.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public void Unexecute()
        {
            if (deletedItem != null && deletedIndex.HasValue)
            {
                // Вставляем элемент обратно на прежнюю позицию
                AppInfo.Todos.Insert(deletedIndex.Value, deletedItem);
                FileManager.SaveTodos(AppInfo.Todos, Program.TodoFilePath);
            }
        }
    }
}

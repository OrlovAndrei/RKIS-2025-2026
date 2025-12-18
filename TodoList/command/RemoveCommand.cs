using System;

namespace TodoApp.Commands
{
    public class RemoveCommand : BaseCommand
    {
        public override string Name => "remove";
        public override string Description => "Удалить задачу";

        // Индекс задачи
        public int TaskIndex { get; set; }

        // Флаг для принудительного удаления
        public bool Force { get; set; }
        
        // Для отмены
        private TodoItem removedItem;
        private int removedIndex = -1;

        public override bool Execute()
        {
            if (AppInfo.Todos == null)
            {
                Console.WriteLine(" Ошибка: TodoList не установлен");
                return false;
            }

            if (AppInfo.Todos.IsEmpty)
            {
                Console.WriteLine(" Список задач пуст!");
                return false;
            }

            try
            {
                removedIndex = TaskIndex - 1;
                removedItem = AppInfo.Todos.GetItem(removedIndex);
                string shortText = GetShortText(removedItem.Text);
                
                if (Force)
                {
                    AppInfo.Todos.Delete(removedIndex);
                    Console.WriteLine(" Задача успешно удалена!");
                    
                    // Сохраняем команду в стек undo
                    PushToUndoStack();
                    AutoSave();
                    return true;
                }
                else
                {
                    Console.Write($" Вы уверены, что хотите удалить задачу '{shortText}'? (y/n): ");
                    string confirmation = Console.ReadLine()?.Trim().ToLower();
                    
                    if (confirmation == "y" || confirmation == "yes" || confirmation == "д" || confirmation == "да")
                    {
                        AppInfo.Todos.Delete(removedIndex);
                        Console.WriteLine("Задача успешно удалена!");
                        
                        // Сохраняем команду в стек undo
                        PushToUndoStack();
                        AutoSave();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine(" Удаление отменено.");
                        return true;
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($" Ошибка: задача с номером {TaskIndex} не найдена!");
                return false;
            }
        }

        public override bool Unexecute()
        {
            if (removedItem != null && AppInfo.Todos != null)
            {
                try
                {
                    // Восстанавливаем удаленную задачу на прежнее место
                    // Для простоты добавляем в конец, но можно реализовать вставку по индексу
                    AppInfo.Todos.Add(removedItem);
                    Console.WriteLine($" Отмена: восстановлена задача '{GetShortText(removedItem.Text)}'");
                    AutoSave();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Ошибка при отмене удаления: {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        private string GetShortText(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            string shortText = text.Replace("\n", " ").Replace("\r", " ");
            return shortText.Length > 30 ? shortText.Substring(0, 30) + "..." : shortText;
        }
    }
}

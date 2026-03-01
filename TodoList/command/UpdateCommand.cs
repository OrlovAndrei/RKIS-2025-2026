using System;

namespace TodoApp.Commands
{
    public class UpdateCommand : BaseCommand
    {
        public override string Name => "update";
        public override string Description => "Обновить текст задачи";

        // Индекс задачи и новый текст
        public int TaskIndex { get; set; }
        public string NewText { get; set; }
        
        // Для отмены
        private string oldText;
        private int actualIndex = -1;

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
                if (!string.IsNullOrWhiteSpace(NewText))
                {
                    // Получаем задачу и сохраняем старый текст
                    actualIndex = TaskIndex - 1;
                    var task = AppInfo.Todos.GetItem(actualIndex);
                    oldText = task.Text;
                    
                    // Обновляем текст
                    task.UpdateText(NewText);
                    Console.WriteLine($" Задача #{TaskIndex} успешно обновлена!");
                    
                    // Сохраняем команду в стек undo
                    PushToUndoStack();
                    AutoSave();
                    return true;
                }
                else
                {
                    Console.WriteLine(" Ошибка: текст задачи не может быть пустым!");
                    return false;
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
            if (actualIndex >= 0 && AppInfo.Todos != null)
            {
                try
                {
                    // Восстанавливаем старый текст
                    var task = AppInfo.Todos.GetItem(actualIndex);
                    string currentText = task.Text;
                    task.UpdateText(oldText);
                    Console.WriteLine($" Отмена: восстановлен текст задачи #{actualIndex + 1}");
                    AutoSave();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Ошибка при отмене обновления: {ex.Message}");
                    return false;
                }
            }
            return false;
        }
    }
}

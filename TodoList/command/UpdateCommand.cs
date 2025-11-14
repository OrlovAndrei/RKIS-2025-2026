using System;

namespace TodoApp.Commands
{
    public class UpdateCommand : ICommand
    {
        public string Name => "update";
        public string Description => "Обновить текст задачи";

        // Индекс задачи и новый текст
        public int TaskIndex { get; set; }
        public string NewText { get; set; }

        // Свойства для работы с данными
        public TodoList TodoList { get; set; }
        public string TodoFilePath { get; set; } // Новое свойство для пути

        public bool Execute()
        {
            if (TodoList == null)
            {
                Console.WriteLine(" Ошибка: TodoList не установлен");
                return false;
            }

            if (TodoList.IsEmpty)
            {
                Console.WriteLine(" Список задач пуст!");
                return false;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(NewText))
                {
                    // Получаем задачу через GetItem() и вызываем метод UpdateText()
                    TodoItem task = TodoList.GetItem(TaskIndex - 1);
                    task.UpdateText(NewText);
                    Console.WriteLine($" Задача #{TaskIndex} успешно обновлена!");
                    
                    // Автосохранение после успешного обновления
                    if (!string.IsNullOrEmpty(TodoFilePath))
                    {
                        FileManager.SaveTodos(TodoList, TodoFilePath);
                    }
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
    }
}

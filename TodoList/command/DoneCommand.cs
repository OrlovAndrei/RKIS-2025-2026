using System;

namespace TodoApp.Commands
{
    public class DoneCommand : ICommand
    {
        public string Name => "done";
        public string Description => "Отметить задачу как выполненную";

        // Индекс задачи
        public int TaskIndex { get; set; }

        // Свойства для работы с данными
        public TodoList TodoList { get; set; }

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
                // Получаем задачу через GetItem() и вызываем метод MarkDone()
                TodoItem task = TodoList.GetItem(TaskIndex - 1);
                
                if (task.IsDone)
                {
                    Console.WriteLine("Эта задача уже отмечена как выполненная.");
                    return true;
                }
                else
                {
                    task.MarkDone();
                    Console.WriteLine($" Задача #{TaskIndex} отмечена как выполненная!");
                    return true;
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
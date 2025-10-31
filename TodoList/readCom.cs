using System;

namespace TodoApp.Commands
{
    public class ReadCommand : ICommand
    {
        public string Name => "read";
        public string Description => "Показать полную информацию о задаче";

        // Индекс задачи
        public int TaskIndex { get; set; }

        // Свойства для работы с данными
        public TodoList TodoList { get; set; }

        public bool Execute()
        {
            if (TodoList == null)
            {
                Console.WriteLine("❌ Ошибка: TodoList не установлен");
                return false;
            }

            if (TodoList.IsEmpty)
            {
                Console.WriteLine("📝 Список задач пуст!");
                return false;
            }

            try
            {
                // Получаем задачу через GetItem() и вызываем метод GetFullInfo()
                TodoItem task = TodoList.GetItem(TaskIndex - 1);
                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine($"ПОЛНАЯ ИНФОРМАЦИЯ О ЗАДАЧЕ #{TaskIndex}");
                Console.WriteLine(new string('=', 60));
                Console.WriteLine(task.GetFullInfo());
                Console.WriteLine(new string('=', 60));
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"❌ Ошибка: задача с номером {TaskIndex} не найдена!");
                return false;
            }
        }
    }
}
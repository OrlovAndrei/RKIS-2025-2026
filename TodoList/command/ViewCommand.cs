using System;

namespace TodoApp.Commands
{
    public class ViewCommand : ICommand
    {
        public string Name => "view";
        public string Description => "Просмотреть список задач";

        // Флаги команд как свойства bool
        public bool ShowIndex { get; set; } = true;
        public bool ShowStatus { get; set; } = true;
        public bool ShowDate { get; set; } = true;

        // Свойства для работы с данными
        public TodoList TodoList { get; set; }

        public bool Execute()
        {
            if (TodoList == null)
            {
                Console.WriteLine("Ошибка: TodoList не установлен");
                return false;
            }

            if (TodoList.IsEmpty)
            {
                Console.WriteLine("Список задач пуст!");
                return true;
            }

            Console.WriteLine("\n=== ВАШИ ЗАДАЧИ ===");
            TodoList.View(ShowIndex, ShowStatus, ShowDate);
            return true;
        }
    }
}
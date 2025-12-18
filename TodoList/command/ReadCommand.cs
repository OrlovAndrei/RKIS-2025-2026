using System;

namespace TodoApp.Commands
{
    public class ReadCommand : BaseCommand
    {
        public override string Name => "read";
        public override string Description => "Показать полную информацию о задаче";

        public int TaskIndex { get; set; }

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
                var task = AppInfo.Todos.GetItem(TaskIndex - 1);
                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine($"ПОЛНАЯ ИНФОРМАЦИЯ О ЗАДАЧЕ #{TaskIndex}");
                Console.WriteLine(new string('=', 60));
                Console.WriteLine(task.GetFullInfo());
                Console.WriteLine(new string('=', 60));
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($" Ошибка: задача с номером {TaskIndex} не найдена!");
                return false;
            }
        }

        // Реализация метода Unexecute для ReadCommand
        public override bool Unexecute()
        {
            // ReadCommand не изменяет состояние, поэтому отмена ничего не делает
            Console.WriteLine(" Команда 'read' не поддерживает отмену.");
            return true;
        }
    }
}

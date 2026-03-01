// Пример для ViewCommand.cs
namespace TodoApp.Commands
{
    public class ViewCommand : BaseCommand
    {
        public override string Name => "view";
        public override string Description => "Просмотреть список задач";

        public bool ShowIndex { get; set; } = true;
        public bool ShowStatus { get; set; } = true;
        public bool ShowDate { get; set; } = true;

        public override bool Execute()
        {
            if (AppInfo.Todos == null)
            {
                Console.WriteLine("Ошибка: TodoList не установлен");
                return false;
            }

            if (AppInfo.Todos.IsEmpty)
            {
                Console.WriteLine("Список задач пуст!");
                return true;
            }

            Console.WriteLine("\n=== ВАШИ ЗАДАЧИ ===");
            AppInfo.Todos.View(ShowIndex, ShowStatus, ShowDate);
            return true;
        }

        public override bool Unexecute()
        {
            // Команда view не изменяет состояние, поэтому отмена не требуется
            return true;
        }
    }
}

// Аналогично для: HelpCommand, ReadCommand, ModifyCommand, ExitCommand
// Они не должны вызывать PushToUndoStack() в методе Execute()

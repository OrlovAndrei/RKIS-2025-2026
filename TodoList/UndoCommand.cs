using System;

namespace TodoApp.Commands
{
    public class UndoCommand : BaseCommand
    {
        public override string Name => "undo";
        public override string Description => "Отменить последнее действие";

        public override bool Execute()
        {
            if (AppInfo.UndoStack.Count == 0)
            {
                Console.WriteLine(" Нет действий для отмены.");
                return true;
            }

            try
            {
                // Берем последнюю команду из стека undo
                var command = AppInfo.UndoStack.Pop();
                
                // Выполняем отмену
                if (command.Unexecute())
                {
                    // Помещаем команду в стек redo
                    AppInfo.RedoStack.Push(command);
                    Console.WriteLine(" Действие отменено.");
                    return true;
                }
                else
                {
                    Console.WriteLine(" Не удалось отменить действие.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка при отмене действия: {ex.Message}");
                return false;
            }
        }

        public override bool Unexecute()
        {
            // Для undo команды отмена не предусмотрена
            Console.WriteLine(" Нельзя отменить команду undo.");
            return false;
        }
    }
}
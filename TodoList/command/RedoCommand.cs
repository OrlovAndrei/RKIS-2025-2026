using System;

namespace TodoApp.Commands
{
    public class RedoCommand : BaseCommand
    {
        public override string Name => "redo";
        public override string Description => "Повторить последнее отмененное действие";

        public override bool Execute()
        {
            if (AppInfo.RedoStack.Count == 0)
            {
                Console.WriteLine(" Нет действий для повторения.");
                return true;
            }

            try
            {
                // Берем последнюю команду из стека redo
                var command = AppInfo.RedoStack.Pop();
                
                // Выполняем команду снова
                if (command.Execute())
                {
                    // Помещаем команду обратно в стек undo
                    AppInfo.UndoStack.Push(command);
                    Console.WriteLine(" Действие повторено.");
                    return true;
                }
                else
                {
                    Console.WriteLine(" Не удалось повторить действие.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка при повторении действия: {ex.Message}");
                return false;
            }
        }

        public override bool Unexecute()
        {
            // Для redo команды отмена не предусмотрена
            Console.WriteLine(" Нельзя отменить команду redo.");
            return false;
        }
    }
}
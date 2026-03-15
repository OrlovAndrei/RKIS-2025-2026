using System;

namespace Todolist
{
    public class ExitCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.CurrentProfileId.HasValue)
            {
                try
                {
                    AppInfo.DataStorage.SaveTodos(AppInfo.CurrentProfileId.Value, AppInfo.GetCurrentTodos());
                    Console.WriteLine("Задачи сохранены.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Предупреждение: не удалось сохранить задачи: {ex.Message}");
                }
            }
            
            Console.WriteLine("Завершение работы приложения...");
            Environment.Exit(0);
        }

        public void Unexecute()
        {
            throw new NotImplementedException("Команда exit не поддерживает отмену");
        }
    }
}
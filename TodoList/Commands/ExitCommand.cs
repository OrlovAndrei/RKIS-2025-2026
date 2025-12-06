using System;

namespace Todolist
{
    public class ExitCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.CurrentProfileId.HasValue)
            {

                FileManager.SaveTodos(AppInfo.GetCurrentTodos(), AppInfo.CurrentProfileId.Value);
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
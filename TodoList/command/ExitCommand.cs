using System;

namespace TodoApp.Commands
{
    public class ExitCommand : BaseCommand
    {
        public override string Name => "exit";
        public override string Description => "Выйти из программы";

        public override bool Execute()
        {
            Console.WriteLine(" Сохраняем данные перед выходом...");
            
            // Сохраняем задачи
            if (AppInfo.Todos != null)
            {
                FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodosFilePath);
            }
            
            // Сохраняем профиль
            if (AppInfo.CurrentProfile != null)
            {
                FileManager.SaveProfile(AppInfo.CurrentProfile, AppInfo.ProfileFilePath);
            }
            
            Console.WriteLine(" Данные сохранены. До свидания!");
            Environment.Exit(0);
            return true;
        }

        // Реализация метода Unexecute для ExitCommand
        public override bool Unexecute()
        {
            // ExitCommand не поддерживает отмену
            Console.WriteLine(" Команда 'exit' не поддерживает отмену.");
            return true;
        }
    }
}
//⣼⣯⠄⣸⣠⣶⣶⣦⣾⠄⡅⡅⠄⠄⠄⠄⡉⠹⠄⡅⠄⠄⠄
//⠿⠿⠶⠿⢿⣿⣿⣿⣿⣦⣤⣄⢀⡅⢠⣾⣛⡉⠄⠄⠄⠸⢀
//⣴⣶⣶⡀⠄⠄⠙⢿⣿⣿⣿⣿⣿⣴⣿⣿⣿⢃⣤⣄⣀⣥⣿
//⣿⣿⣿⣧⣀⢀⣠⡌⢻⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⠿⠿⣿⣿
//⣤⣤⣤⣬⣙⣛⢿⣿⣿⣿⣿⣿⣿⡿⣿⣿⡍⠄⠄⢀⣤⣄⠉
//⣿⣿⣿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⢇⣿⣿⡷⠶⠶⢿⣿⣿⠇
//⣿⣿⣿⣿⣿⣿⣿⣿⣽⣿⣿⣿⡇⣿⣿⣿⣿⣿⣿⣷⣶⣥⣴
//⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
//⣻⣿⣿⣧⠙⠛⠛⡭⠅⠒⠦⠭⣭⡻⣿⣿⣿⣿⣿⣿⣿⣿⡿
//⣿⣿⣿⣿⡆⠄⠄⠄⠄⠄⠄⠄⠄⠹⠈⢋⣽⣿⣿⣿⣿⣵⣾
//⣿⣿⣿⣿⣿⠄⣴⣿⣶⣄⠄⣴⣶⠄⢀⣾⣿⣿⣿⣿⣿⣿⠃
//⠛⢿⣿⣿⣿⣦⠁⢿⣿⣿⡄⢿⣿⡇⣸⣿⣿⠿⠛⠁⠄⠄⠄
//⠄⠄⠉⠻⣿⣿⣿⣦⡙⠻⣷⣾⣿⠃⠿⠋⠁⠄⠄⠄⠄⠄⢀
//⣮⣥⠄⠄⠄⠛⢿⣿⣿⡆⣿⡿⠃⠄⠄⠄⠄⠄⠄⠄⣠⣴⣿


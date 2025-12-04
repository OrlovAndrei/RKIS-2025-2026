using System;

namespace Todolist.Commands
{
    internal class ProfileCommand : ICommand
    {
        private Profile? oldProfile = null;
        private bool wasUpdated = false;
        private readonly string args;

        public ProfileCommand(string args)
        {
            this.args = args ?? string.Empty;
        }

        public void Execute()
        {
            // Обработка выхода из профиля: profile --out / -o
            if (args.Contains("--out", StringComparison.OrdinalIgnoreCase) ||
                args.Contains("-o", StringComparison.OrdinalIgnoreCase))
            {
                // Сохраняем данные текущего профиля и его задач
                FileManager.SaveProfiles(AppInfo.Profiles, Program.ProfileFilePath);
                FileManager.SaveTodos(AppInfo.Todos, Program.TodoFilePath);

                // Сброс текущего профиля и стеков undo/redo
                AppInfo.CurrentProfileId = Guid.Empty;
                AppInfo.UndoStack.Clear();
                AppInfo.RedoStack.Clear();

                Console.WriteLine("Вы вышли из профиля.\n");

                // Возврат к выбору профиля
                Program.SelectProfile();
                return;
            }

            Console.WriteLine("Текущий профиль: " + AppInfo.CurrentProfile.GetInfo());
            Console.Write("Хотите обновить данные? (y/n): ");
            string answer = Console.ReadLine()?.Trim().ToLower() ?? "n";
            
            if (answer == "y")
            {
                string login = Program.Prompt("Введите логин: ") ?? string.Empty;
                string password = Program.Prompt("Введите пароль: ") ?? string.Empty;
                string firstName = Program.Prompt("Введите имя: ") ?? string.Empty;
                string lastName = Program.Prompt("Введите фамилию: ") ?? string.Empty;
                int birthYear = Program.ReadInt("Введите год рождения: ");

                // Сохраняем старый профиль для отмены
                oldProfile = new Profile(
                    AppInfo.CurrentProfile.Id,
                    AppInfo.CurrentProfile.Login,
                    AppInfo.CurrentProfile.Password,
                    AppInfo.CurrentProfile.FirstName,
                    AppInfo.CurrentProfile.LastName,
                    AppInfo.CurrentProfile.BirthYear
                );

                // Обновляем текущий профиль в списке
                var current = AppInfo.CurrentProfile;
                current.Login = login;
                current.Password = password;
                current.FirstName = firstName;
                current.LastName = lastName;
                current.BirthYear = birthYear;

                FileManager.SaveProfiles(AppInfo.Profiles, Program.ProfileFilePath);
                Console.WriteLine($"\nПрофиль обновлён: {AppInfo.CurrentProfile.GetInfo()}");
                wasUpdated = true;
            }
        }

        public void Unexecute()
        {
            if (wasUpdated && oldProfile != null)
            {
                // Находим профиль в списке и откатываем изменения
                var current = AppInfo.Profiles.Find(p => p.Id == oldProfile.Id);
                if (current != null)
                {
                    current.Login = oldProfile.Login;
                    current.Password = oldProfile.Password;
                    current.FirstName = oldProfile.FirstName;
                    current.LastName = oldProfile.LastName;
                    current.BirthYear = oldProfile.BirthYear;
                    AppInfo.CurrentProfile = current;
                }

                FileManager.SaveProfiles(AppInfo.Profiles, Program.ProfileFilePath);
            }
        }
    }
}

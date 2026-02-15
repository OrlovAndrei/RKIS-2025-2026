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
            if (args.Contains("--out", StringComparison.OrdinalIgnoreCase) ||
                args.Contains("-o", StringComparison.OrdinalIgnoreCase))
            {
                FileManager.SaveProfiles(AppInfo.Profiles, Program.ProfileFilePath);
                FileManager.SaveTodos(AppInfo.Todos, Program.TodoFilePath);

                AppInfo.CurrentProfileId = Guid.Empty;
                AppInfo.UndoStack.Clear();
                AppInfo.RedoStack.Clear();

                Console.WriteLine("Вы вышли из профиля.\n");
                Program.SelectProfile();
                return;
            }

            Console.WriteLine("Текущий профиль: " + AppInfo.CurrentProfile.GetInfo());
            Console.Write("Изменить данные профиля? (y/n): ");
            string answer = Console.ReadLine()?.Trim().ToLower() ?? "n";
            
            if (answer == "y")
            {
                string login = Program.Prompt("Логин: ") ?? string.Empty;
                string password = Program.Prompt("Пароль: ") ?? string.Empty;
                string firstName = Program.Prompt("Имя: ") ?? string.Empty;
                string lastName = Program.Prompt("Фамилия: ") ?? string.Empty;
                int birthYear = Program.ReadInt("Год рождения: ");

                oldProfile = new Profile(
                    AppInfo.CurrentProfile.Id,
                    AppInfo.CurrentProfile.Login,
                    AppInfo.CurrentProfile.Password,
                    AppInfo.CurrentProfile.FirstName,
                    AppInfo.CurrentProfile.LastName,
                    AppInfo.CurrentProfile.BirthYear
                );

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


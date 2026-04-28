using System;
using Todolist.Exceptions;

namespace Todolist.Commands
{
    internal class ProfileCommand : IUndo
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
                AppInfo.Storage.SaveProfiles(AppInfo.Profiles);
                if (AppInfo.CurrentProfileId != Guid.Empty)
                    AppInfo.Storage.SaveTodos(AppInfo.CurrentProfileId, AppInfo.Todos);

                AppInfo.CurrentProfileId = Guid.Empty;
                AppInfo.UndoStack.Clear();
                AppInfo.RedoStack.Clear();

                Console.WriteLine("Вы вышли из профиля.\n");
                Program.SelectProfile();
                return;
            }

            Console.WriteLine("Текущий профиль: " + AppInfo.CurrentProfile.GetInfo());
            Console.Write("Изменить данные профиля? (y/n): ");
            string answer = Console.ReadLine()?.Trim().ToLowerInvariant() ?? "n";

            if (answer == "y")
            {
                string login = Program.Prompt("Логин: ").Trim();
                if (string.IsNullOrWhiteSpace(login))
                    throw new InvalidArgumentException("Логин не может быть пустым.");

                var existingWithLogin = AppInfo.Profiles.Find(p =>
                    string.Equals(p.Login, login, StringComparison.OrdinalIgnoreCase) &&
                    p.Id != AppInfo.CurrentProfile.Id);
                if (existingWithLogin != null)
                    throw new DuplicateLoginException("Пользователь с таким логином уже зарегистрирован.");

                string password = Program.Prompt("Пароль: ");
                string firstName = Program.Prompt("Имя: ");
                string lastName = Program.Prompt("Фамилия: ");
                int birthYear = Program.ReadInt("Год рождения: ");
                int currentYear = DateTime.Now.Year;
                if (birthYear < 1900 || birthYear > currentYear)
                    throw new InvalidArgumentException($"Год рождения должен быть в диапазоне 1900-{currentYear}.");

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

                AppInfo.Storage.SaveProfiles(AppInfo.Profiles);
                Console.WriteLine($"\nПрофиль обновлён: {AppInfo.CurrentProfile.GetInfo()}");
                wasUpdated = true;
            }
        }

        public void Unexecute()
        {
            if (!wasUpdated || oldProfile == null)
                return;

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

            AppInfo.Storage.SaveProfiles(AppInfo.Profiles);
        }
    }
}

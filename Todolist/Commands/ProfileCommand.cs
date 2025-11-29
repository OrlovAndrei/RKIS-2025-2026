using System;

namespace Todolist.Commands
{
    internal class ProfileCommand : ICommand
    {
        private Profile? oldProfile = null;
        private bool wasUpdated = false;

        public ProfileCommand()
        {
        }

        public void Execute()
        {
            Console.WriteLine("Текущий профиль: " + AppInfo.CurrentProfile.GetInfo());
            Console.Write("Хотите обновить данные? (y/n): ");
            string answer = Console.ReadLine()?.Trim().ToLower() ?? "n";
            
            if (answer == "y")
            {
                string firstName = Program.Prompt("Введите имя: ") ?? string.Empty;
                string lastName = Program.Prompt("Введите фамилию: ") ?? string.Empty;
                int birthYear = Program.ReadInt("Введите год рождения: ");

                // Сохраняем старый профиль для отмены
                oldProfile = new Profile(
                    AppInfo.CurrentProfile.FirstName,
                    AppInfo.CurrentProfile.LastName,
                    AppInfo.CurrentProfile.BirthYear
                );

                AppInfo.CurrentProfile = new Profile(firstName, lastName, birthYear);
                FileManager.SaveProfile(AppInfo.CurrentProfile, Program.ProfileFilePath);
                Console.WriteLine($"\nПрофиль обновлён: {AppInfo.CurrentProfile.GetInfo()}");
                wasUpdated = true;
            }
        }

        public void Unexecute()
        {
            if (wasUpdated && oldProfile != null)
            {
                AppInfo.CurrentProfile = oldProfile;
                FileManager.SaveProfile(AppInfo.CurrentProfile, Program.ProfileFilePath);
            }
        }
    }
}

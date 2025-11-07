using System;

namespace Todolist.Commands
{
    internal class ProfileCommand : ICommand
    {
        public Profile Profile { get; set; }

        public ProfileCommand(Profile profile)
        {
            Profile = profile;
        }

        public void Execute()
        {
            Console.WriteLine("Текущий профиль: " + Profile.GetInfo());
            Console.Write("Хотите обновить данные? (y/n): ");
            string answer = Console.ReadLine()?.Trim().ToLower() ?? "n";
            
            if (answer == "y")
            {
                string firstName = Program.Prompt("Введите имя: ") ?? string.Empty;
                string lastName = Program.Prompt("Введите фамилию: ") ?? string.Empty;
                int birthYear = Program.ReadInt("Введите год рождения: ");

                Profile = new Profile(firstName, lastName, birthYear);
                FileManager.SaveProfile(Profile, Program.ProfileFilePath);
                Console.WriteLine($"\nПрофиль обновлён: {Profile.GetInfo()}");
            }
        }
    }
}

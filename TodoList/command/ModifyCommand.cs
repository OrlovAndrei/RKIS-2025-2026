using System;

namespace TodoApp.Commands
{
    public class ModifyCommand : ICommand
    {
        public string Name => "modify";
        public string Description => "Показать профиль пользователя";

        // Свойства для работы с данными
        public Profile UserProfile { get; set; }
        public string ProfileFilePath { get; set; } // Новое свойство для пути

        public bool Execute()
        {
            if (UserProfile == null)
            {
                Console.WriteLine(" Ошибка: Profile не установлен");
                return false;
            }

            Console.WriteLine("\n=== ВАШ ПРОФИЛЬ ===");
            Console.WriteLine(UserProfile.GetInfo());
            
            // Если нужно редактирование профиля, можно добавить здесь:
            Console.Write("\nХотите изменить профиль? (y/n): ");
            string response = Console.ReadLine()?.Trim().ToLower();
            
            if (response == "y" || response == "yes" || response == "д" || response == "да")
            {
                return EditProfile();
            }

            return true;
        }

        private bool EditProfile()
        {
            Console.WriteLine("\n=== РЕДАКТИРОВАНИЕ ПРОФИЛЯ ===");
            
            Console.Write("Введите новое имя: ");
            string firstName = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(firstName))
            {
                UserProfile.FirstName = firstName;
            }

            Console.Write("Введите новую фамилию: ");
            string lastName = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(lastName))
            {
                UserProfile.LastName = lastName;
            }

            Console.Write("Введите новый год рождения: ");
            if (int.TryParse(Console.ReadLine(), out int birthYear) && birthYear > 1900 && birthYear <= DateTime.Now.Year)
            {
                UserProfile.BirthYear = birthYear;
            }

            Console.WriteLine($" Профиль обновлен: {UserProfile.GetInfo()}");
            
            // Автосохранение профиля после изменения
            if (!string.IsNullOrEmpty(ProfileFilePath))
            {
                FileManager.SaveProfile(UserProfile, ProfileFilePath);
            }
            
            return true;
        }
    }
}

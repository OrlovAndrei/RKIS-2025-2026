using System;

namespace TodoApp.Commands
{
    public class ModifyCommand : BaseCommand
    {
        public override string Name => "modify";
        public override string Description => "Показать и изменить профиль пользователя";

        // Для отмены изменений профиля
        private string oldFirstName;
        private string oldLastName;
        private int oldBirthYear;

        public override bool Execute()
        {
            if (AppInfo.CurrentProfile == null)
            {
                Console.WriteLine(" Ошибка: Profile не установлен");
                return false;
            }

            Console.WriteLine("\n=== ВАШ ПРОФИЛЬ ===");
            Console.WriteLine(AppInfo.CurrentProfile.GetInfo());
            
            // Сохраняем старые данные для возможной отмены
            oldFirstName = AppInfo.CurrentProfile.FirstName;
            oldLastName = AppInfo.CurrentProfile.LastName;
            oldBirthYear = AppInfo.CurrentProfile.BirthYear;

            Console.Write("\nХотите изменить профиль? (y/n): ");
            string response = Console.ReadLine()?.Trim().ToLower();
            
            if (response == "y" || response == "yes" || response == "д" || response == "да")
            {
                return EditProfile();
            }

            return true;
        }

        public override bool Unexecute()
        {
            if (AppInfo.CurrentProfile != null)
            {
                // Восстанавливаем старые данные профиля
                AppInfo.CurrentProfile.FirstName = oldFirstName;
                AppInfo.CurrentProfile.LastName = oldLastName;
                AppInfo.CurrentProfile.BirthYear = oldBirthYear;
                
                Console.WriteLine(" Отмена: восстановлены старые данные профиля");
                
                // Автосохранение профиля после отмены
                FileManager.SaveProfile(AppInfo.CurrentProfile, AppInfo.ProfileFilePath);
                
                return true;
            }
            return false;
        }

        private bool EditProfile()
        {
            Console.WriteLine("\n=== РЕДАКТИРОВАНИЕ ПРОФИЛЯ ===");
            
            Console.Write("Введите новое имя: ");
            string firstName = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(firstName))
            {
                AppInfo.CurrentProfile.FirstName = firstName;
            }

            Console.Write("Введите новую фамилию: ");
            string lastName = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(lastName))
            {
                AppInfo.CurrentProfile.LastName = lastName;
            }

            Console.Write("Введите новый год рождения: ");
            if (int.TryParse(Console.ReadLine(), out int birthYear) && birthYear > 1900 && birthYear <= DateTime.Now.Year)
            {
                AppInfo.CurrentProfile.BirthYear = birthYear;
            }

            Console.WriteLine($" Профиль обновлен: {AppInfo.CurrentProfile.GetInfo()}");
            
            // Сохраняем команду в стек undo
            PushToUndoStack();
            
            // Автосохранение профиля после изменения
            FileManager.SaveProfile(AppInfo.CurrentProfile, AppInfo.ProfileFilePath);
            
            return true;
        }
    }
}

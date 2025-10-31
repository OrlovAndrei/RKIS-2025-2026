using System;

namespace TodoApp.Commands
{
    public class ModifyCommand : ICommand
    {
        public string Name => "modify";
        public string Description => "Показать профиль пользователя";

        // Свойства для работы с данными
        public Profile UserProfile { get; set; }

        public bool Execute()
        {
            if (UserProfile == null)
            {
                Console.WriteLine("❌ Ошибка: Profile не установлен");
                return false;
            }

            Console.WriteLine("\n=== ВАШ ПРОФИЛЬ ===");
            Console.WriteLine(UserProfile.GetInfo());
            return true;
        }
    }
}
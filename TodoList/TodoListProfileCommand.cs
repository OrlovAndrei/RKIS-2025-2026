using System;
using System.IO;

namespace TodoList
{
    /// <summary>
    /// Команда вывода информации о профиле пользователя.
    /// </summary>
    internal class ProfileCommand : ICommand
    {
        public Profile? Profile { get; set; }
        public string? ProfileFilePath { get; set; }

        public void Execute()
        {
            if (Profile == null)
            {
                Console.WriteLine("Ошибка: профиль не установлен.");
                return;
            }

            Console.WriteLine(Profile.GetInfo());

            // Сохраняем профиль после вывода (на случай если он был изменен)
            if (!string.IsNullOrWhiteSpace(ProfileFilePath))
            {
                FileManager.SaveProfile(Profile, ProfileFilePath);
            }
        }
    }
}


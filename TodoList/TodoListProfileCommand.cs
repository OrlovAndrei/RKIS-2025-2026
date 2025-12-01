using System;

namespace TodoList
{
    /// <summary>
    /// Команда вывода информации о профиле пользователя.
    /// </summary>
    internal class ProfileCommand : ICommand
    {
        public Profile? Profile { get; set; }

        public void Execute()
        {
            if (Profile == null)
            {
                Console.WriteLine("Ошибка: профиль не установлен.");
                return;
            }

            Console.WriteLine(Profile.GetInfo());
        }
    }
}


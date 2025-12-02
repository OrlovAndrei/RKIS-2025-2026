using System;
using System.IO;

namespace TodoList
{
    /// <summary>
    /// Команда вывода информации о профиле пользователя.
    /// </summary>
    internal class ProfileCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.CurrentProfile == null)
            {
                Console.WriteLine("Ошибка: профиль не установлен.");
                return;
            }

            Console.WriteLine(AppInfo.CurrentProfile.GetInfo());
        }

        public void Unexecute()
        {
            // Команда profile ничего не изменяет
        }
    }
}


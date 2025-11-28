using System;

namespace Todolist
{
    public class ProfileCommand : ICommand
    {
        public Profile UserProfile { get; private set; }

        public ProfileCommand(Profile userProfile)
        {
            UserProfile = userProfile;
        }

        public void Execute()
        {
            if (UserProfile != null)
            {
                Console.WriteLine("=== Информация о профиле ===");
                Console.WriteLine(UserProfile.GetInfo());
                Console.WriteLine("=============================");
            }
            else
            {
                Console.WriteLine("Данные пользователя не найдены");
            }
        }

        public void Unexecute()
        {
            throw new NotImplementedException("Команда profile не поддерживает отмену");
        }
    }
}
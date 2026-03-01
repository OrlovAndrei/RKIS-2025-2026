using System;
using Todolist.Exceptions;

namespace Todolist
{
    public class ProfileCommand : ICommand
    {
        private readonly bool _isLogout;

        public ProfileCommand(bool isLogout = false)
        {
            _isLogout = isLogout;
        }

        public void Execute()
        {
            if (_isLogout)
            {
                Logout();
            }
            else
            {
                ShowProfile();
            }
        }

        private void ShowProfile()
        {
            var userProfile = AppInfo.GetCurrentProfile();

            if (userProfile == null)
                throw new ProfileNotFoundException("Профиль не найден");

            Console.WriteLine("=== Информация о профиле ===");
            Console.WriteLine(userProfile.GetInfo());
            Console.WriteLine("=============================");
        }

        private void Logout()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
                throw new AuthenticationException("Вы не вошли в профиль. Выход невозможен.");

            var currentProfile = AppInfo.GetCurrentProfile();
            FileManager.SaveTodos(AppInfo.GetCurrentTodos(), AppInfo.CurrentProfileId.Value);

            Console.WriteLine($"Вы вышли из профиля пользователя: {currentProfile?.Login}");
            AppInfo.CurrentProfileId = null;
            AppInfo.ClearUndoRedoStacks();
        }

        public void Unexecute()
        {
            throw new NotImplementedException("Команда profile не поддерживает отмену");
        }
    }
}
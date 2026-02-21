using System;
using System.Linq;
using TodoList.Exceptions;

namespace TodoList.Commands
{
    public class ProfileCommand : ICommand
    {
        public string[] Flags { get; set; } = Array.Empty<string>();

        public void Execute()
        {
            if (Flags.Contains("out") || Flags.Contains("logout"))
            {
                if (AppInfo.CurrentProfileId == null)
                {
                    throw new AuthenticationException("Вы не в системе.");
                }
                
                Program.SaveCurrentUserTasks();
                Console.WriteLine("Вы вышли из системы.");
                AppInfo.CurrentProfileId = null;
                return;
            }

            if (AppInfo.CurrentProfileId == null)
            {
                throw new AuthenticationException("Вы не вошли в систему.");
            }

            var currentProfile = AppInfo.AllProfiles.FirstOrDefault(p => p.Id == AppInfo.CurrentProfileId);
            if (currentProfile == null)
            {
                throw new ProfileNotFoundException("Профиль не найден.");
            }

            currentProfile.ShowProfile();
        }

        public void Unexecute() { }
    }
}
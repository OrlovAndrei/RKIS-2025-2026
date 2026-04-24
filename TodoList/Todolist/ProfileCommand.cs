using System;
using System.IO;

namespace Todolist
{
    public class ProfileCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine(AppInfo.CurrentProfile.GetInfo());

            string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
            string profileFile = Path.Combine(dataDir, "profile.txt");
            FileManager.SaveProfile(AppInfo.CurrentProfile, profileFile);
        }

        public void Unexecute()
        {
        }
    }
}

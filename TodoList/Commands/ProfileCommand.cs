using System;

namespace TodoList.Commands
{
    public class ProfileCommand : ICommand
    {
        public Profile Profile { get; set; }

        public void Execute()
        {
            Profile.ShowProfile();
            FileManager.SaveProfile(Profile, "data/profile.txt");
        }
    }
}
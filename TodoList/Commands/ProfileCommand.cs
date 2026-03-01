using System;

namespace TodoList
{
    public class ProfileCommand : ICommand
    {
        public Profile Profile { get; private set; }

        public ProfileCommand(Profile profile)
        {
            Profile = profile;
        }

        public void Execute()
        {
            Console.WriteLine($"Пользователь {Profile.GetInfo()}");
        }
    }
}
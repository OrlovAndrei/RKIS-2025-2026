namespace TodoList
{
    public sealed class ProfileCommand : ICommand
    {
        public Profile Profile { get; set; }
        public bool SkipProfile { get; set; }

        public void Execute()
        {
            if (SkipProfile)
            {
                Console.WriteLine("Профиль отключен флагом --skip-profile");
            }
            else
            {
                Console.WriteLine(Profile.GetInfo());
                FileManager.SaveProfile(Profile, "data/profile.txt");
            }
        }
    }
}
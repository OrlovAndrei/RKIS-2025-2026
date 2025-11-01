namespace TodoList
{
    public class ProfileCommand : ICommand
    {
        private readonly Profile profile;

        public ProfileCommand(Profile profile)
        {
            this.profile = profile;
        }

        public void Execute()
        {
            System.Console.WriteLine(profile.GetInfo());
        }
    }
}
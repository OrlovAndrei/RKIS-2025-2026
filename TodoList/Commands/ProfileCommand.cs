namespace TodoList
{
    public class ProfileCommand : ICommand
    {
        private readonly Profile _profile;

        public ProfileCommand(Profile profile)
        {
            _profile = profile;
        }

        public void Execute()
        {
            Console.WriteLine(_profile.GetInfo());
        }

        public void Unexecute() { }
    }
}
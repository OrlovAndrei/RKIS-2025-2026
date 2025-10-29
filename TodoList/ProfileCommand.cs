namespace TodoList
{
    public class ProfileCommand : BaseCommand
    {
        public ProfileCommand(Profile profile) : base(null, profile) { }

        public override void Execute()
        {
            System.Console.WriteLine(Profile.GetInfo());
        }
    }
}
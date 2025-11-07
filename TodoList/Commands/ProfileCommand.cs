namespace TodoList
{
    public class ProfileCommand : ICommand
    {
        public Profile Profile { get; set; } = null!;

        public void Execute()
        {
            Profile.ShowProfile();
        }
    }
}
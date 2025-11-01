class ProfileCommand : ICommand
{
    public Profile Profile { get; set; }

    public ProfileCommand(Profile profile)
    {
        Profile = profile;
    }

    public void Execute()
    {
        Console.WriteLine(Profile.GetInfo());
    }
}


using ConsoleApp.Parser.ProfileCommands;
using ConsoleApp.Parser.Verb;

namespace ConsoleApp.Parser;

internal static class RunProfileCommands
{
    public async static Task RunAdd(ProfileAdd p)
    {
        await AddProfileCommand.Execute(p);
    }

    public async static Task RunRemove(ProfileRemove p)
    {
        await RemoveProfileCommand.Execute(p);
    }

    public async static Task RunChange(ProfileChange p)
    {
        await ChangeProfileCommand.Execute(p);
    }

    public async static Task RunEdit(ProfileEdit p)
    {
        await EditProfileCommand.Execute(p);
    }

    public async static Task RunSearch(ProfileSearch p)
    {
        await SearchProfileCommand.Execute(p);
    }

    public async static Task RunList(ProfileList p)
    {
        await ListProfileCommand.Execute(p);
    }
}

namespace ShevricTodo.Commands.ProfileObj;

internal partial class Show
{
	public static async Task ShowActiveProfile() => await ShowActiveProfile(printPanel: Input.WriteToConsole.PrintPanel);
	public static async Task ShowProfile(
		Database.Profile profile) => await ShowProfile(profile: profile,
			printPanel: Input.WriteToConsole.PrintPanel);
}

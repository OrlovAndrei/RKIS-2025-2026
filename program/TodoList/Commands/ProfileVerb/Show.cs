using ShevricTodo.Authentication;
using System.Text;

namespace ShevricTodo.Commands.ProfileVerb;

internal class Show : Profile
{
	/// <summary>
	/// Asynchronously retrieves the active user profile and displays its information in the specified output panel.
	/// </summary>
	/// <remarks>The method formats and displays the user's ID, username, first and last names, birthday, and
	/// creation date. ProfileVerb fields that are null are omitted from the output. The method ensures that only available
	/// profile details are included.</remarks>
	/// <param name="printPanel">An action that receives a header string and a collection of text lines, used to present the profile information in
	/// the output panel.</param>
	/// <returns>A task that represents the asynchronous operation of printing the active profile information.</returns>
	public static async Task ShowActiveProfile(
		Func<string, IEnumerable<string>, Task> printPanel)
	{
		Database.Profile activeProfile = await ActiveProfile.GetActiveProfile();
		await ShowProfile(printPanel, activeProfile);
	}
	public static async Task ShowActiveProfile()
	{
		await ShowActiveProfile(printPanel: Input.WriteToConsole.PrintPanel);
	}
	/// <summary>
	/// Prints the specified user's profile information to a provided print panel.
	/// </summary>
	/// <remarks>The method constructs a header and a list of profile details based on the available fields in the
	/// profile object. If the username, first name, or last name are present, they are included in the output. Birthday
	/// and creation date are also displayed if available.</remarks>
	/// <param name="printPanel">An action that receives a header string and a collection of text lines, used to display the formatted profile
	/// information.</param>
	/// <param name="profile">The profile object containing user details, including user ID, username, first name, last name, birthday, and
	/// creation date.</param>
	/// <returns>A task that represents the asynchronous operation of printing the profile information.</returns>
	public static async Task ShowProfile(
		Func<string, IEnumerable<string>, Task> printPanel,
		Database.Profile profile)
	{
		StringBuilder header = new($" ID: [{profile.UserId}] ");
		bool availabilityUserName = profile.UserName is not null;
		bool availabilityFirstName = profile.FirstName is not null;
		bool availabilityLastName = profile.LastName is not null;
		List<string> textLinesPanel = new();
		if (availabilityUserName)
		{
			textLinesPanel.Add($"UserName: {profile.UserName}.");
			header.Append($" {profile.UserName} (");
			await GetFullName();
			header.Append(')');
		}
		else
		{
			await GetFullName();
		}
		if (profile.Birthday is not null)
		{ textLinesPanel.Add($"Birthday: {profile.Birthday}."); }
		if (profile.DateOfCreate is not null)
		{ textLinesPanel.Add($"DateOfCreate: {profile.DateOfCreate}."); }
		await printPanel(header.ToString(), textLinesPanel);
		async System.Threading.Tasks.Task GetFullName()
		{
			if (availabilityFirstName)
			{
				textLinesPanel.Add($"FirstName: {profile.FirstName}.");
				header.Append(profile.FirstName);
				if (availabilityLastName)
				{ header.Append(' '); }
			}
			if (availabilityLastName)
			{
				textLinesPanel.Add($"LastName: {profile.LastName}.");
				header.Append(profile.LastName);
			}
		}
	}
	public static async Task ShowProfile(
		Database.Profile profile)
	{
		await ShowProfile(profile: profile,
			printPanel: Input.WriteToConsole.PrintPanel);
	}
}

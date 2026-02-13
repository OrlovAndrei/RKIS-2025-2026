using ShevricTodo.Authentication;

namespace ShevricTodo.Commands.ProfileObj;

internal partial class List : ProfileObj
{
	/// <summary>
	/// Prints a formatted table of user profiles, including information about the active user.
	/// </summary>
	/// <remarks>The method retrieves the active user profile and includes its information in the table title.
	/// Ensure that the profiles collection is not null to avoid runtime errors.</remarks>
	/// <param name="printTable">An action that displays the table, accepting an array of column names, a collection of row values, and an optional
	/// title.</param>
	/// <param name="profiles">A collection of user profiles to be displayed. Each profile must contain user information such as UserId,
	/// FirstName, LastName, UserName, DateOfCreate, and Birthday. Cannot be null.</param>
	/// <returns>A task that represents the asynchronous operation of printing the profiles.</returns>
	private static async Task PrintProfiles(
	Func<string[], IEnumerable<string[]>, string?, Task> printTable,
	IEnumerable<Database.Profile> profiles)
	{
		Database.Profile activeUser = await ActiveProfile.GetActiveProfile();
		string title = $"Active profile {activeUser.UserId}: {activeUser.FirstName} {activeUser.LastName}";
		string[] columns = [
			"UserId",
			"FirstName",
			"LastName",
			"UserName",
			"DataOfCreate",
			"Birthday"];
		IEnumerable<string[]> rows = profiles.Select(p => new string[]
		{
				p.UserId.ToString().NotAvailable(),
				p.FirstName.NotAvailable(),
				p.LastName.NotAvailable(),
				p.UserName.NotAvailable(),
				p.DateOfCreate.ToString().NotAvailable(),
				p.Birthday.ToString().NotAvailable()
		});
		await printTable(columns, rows, title);
	}
	/// <summary>
	/// Asynchronously prints the number of tasks associated with each user profile in a formatted table.
	/// </summary>
	/// <remarks>The method retrieves the active user profile and counts the tasks for each provided profile. The
	/// results are formatted and displayed using the specified table-printing action. The table includes user identifiers,
	/// names, usernames, and task counts for each profile.</remarks>
	/// <param name="printTable">An action that displays the table, accepting an array of column names, a collection of row values, and an optional
	/// title.</param>
	/// <param name="profiles">A collection of user profiles for which task counts will be retrieved and displayed.</param>
	/// <returns>A task that represents the asynchronous operation. This method does not return a value.</returns>
	private static async Task PrintTaskCountsByProfile(
	Func<string[], IEnumerable<string[]>, string?, Task> printTable,
	IEnumerable<Database.Profile> profiles)
	{
		IEnumerable<(int profileId, int countTasks)> taskCountsByProfile =
			await GetTaskCountsByProfile(profiles);
		Database.Profile activeUser = await ActiveProfile.GetActiveProfile();
		string title = $"Active profile[{activeUser.UserId}]: {activeUser.FirstName} {activeUser.LastName}";
		string[] columns = [
			"UserId",
			"FirstName",
			"LastName",
			"UserName",
			"CountTask"];
		IEnumerable<string[]> rows =
			from profileCount in taskCountsByProfile
			join profile in profiles on profileCount.profileId equals profile.UserId
			select new string[]
			{
				profileCount.profileId.ToString(),
				profile.FirstName.NotAvailable(),
				profile.LastName.NotAvailable(),
				profile.UserName.NotAvailable(),
				profileCount.countTasks.ToString()
			}
			.ToArray();
		await printTable(columns, rows, title);
	}
}

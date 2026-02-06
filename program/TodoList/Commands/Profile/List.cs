using ShevricTodo.Authentication;

namespace ShevricTodo.Commands.Profile;

internal class List : Profile
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
	public static async System.Threading.Tasks.Task PrintProfiles(
	Action<string[], IEnumerable<string[]>, string?> printTable,
	IEnumerable<Database.Profile> profiles)
	{
		Database.Profile activeUser = await ActiveProfile.GetActiveProfile();
		string title = $"Active profile[{activeUser.UserId}]: {activeUser.FirstName} {activeUser.LastName}";
		string[] columns = [
			"UserId",
			"FirstName",
			"LastName",
			"UserName",
			"DataOfCreate",
			"Birthday"];
		IEnumerable<string[]> rows =
			from profile in profiles
			select new string[]
			{
				profile.UserId.ToString(),
				profile.FirstName ?? "N/A",
				profile.LastName ?? "N/A",
				profile.UserName ?? "N/A",
				profile.DateOfCreate.ToString() ?? "N/A",
				profile.Birthday.ToString() ?? "N/A"
			}
			.ToArray();
		printTable(columns, rows, title);
	}
	/// <summary>
	/// Asynchronously prints all user profiles in a formatted table, including information about the currently active
	/// user.
	/// </summary>
	/// <remarks>The table includes details for each user profile, such as UserId, FirstName, LastName, UserName,
	/// DateOfCreate, and Birthday. The title of the table highlights the active user's information.</remarks>
	/// <param name="printTable">A delegate that prints the table. Receives an array of column names, an enumerable of row values, and an optional
	/// title string.</param>
	/// <returns>A task that represents the asynchronous print operation.</returns>
	public static async System.Threading.Tasks.Task PrintAllProfiles(
	Action<string[], IEnumerable<string[]>, string?> printTable)
	{
		IEnumerable<Database.Profile> profiles = await GetAllProfile();
		await PrintProfiles(printTable, profiles);
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
	public static async System.Threading.Tasks.Task PrintTaskCountsByProfile(
	Action<string[], IEnumerable<string[]>, string?> printTable,
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
			from profileCout in taskCountsByProfile
			join profile in profiles on profileCout.profileId equals profile.UserId
			select new string[]
			{
				profileCout.profileId.ToString(),
				profile.FirstName ?? "N/A",
				profile.LastName ?? "N/A",
				profile.UserName ?? "N/A",
				profileCout.countTasks.ToString()
			}
			.ToArray();
		printTable(columns, rows, title);
	}
	/// <summary>
	/// Asynchronously prints the task counts for all profiles using the specified table formatting action.
	/// </summary>
	/// <remarks>This method retrieves all profiles and prints their task counts by invoking the provided printTable
	/// action. The caller is responsible for supplying a suitable action to format and display the results.</remarks>
	/// <param name="printTable">An action that defines how to print the task counts. The action receives an array of profile names, an enumerable
	/// of task count arrays for each profile, and an optional string for additional formatting.</param>
	/// <returns>A task that represents the asynchronous print operation.</returns>
	public static async System.Threading.Tasks.Task PrintTaskCountsByAllProfile(
	Action<string[], IEnumerable<string[]>, string?> printTable)
	{
		IEnumerable<Database.Profile> profiles = await GetAllProfile();
		await PrintTaskCountsByProfile(printTable, profiles);
	}
}

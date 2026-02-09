namespace ShevricTodo.Commands.ProfileObj;

internal partial class List
{
	public static async Task PrintProfiles(
	IEnumerable<Database.Profile> profiles)
	{
		await PrintProfiles(
			printTable: Input.WriteToConsole.PrintTable,
			profiles: profiles);
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
	public static async Task PrintAllProfiles(
	Func<string[], IEnumerable<string[]>, string?, Task> printTable)
	{
		IEnumerable<Database.Profile> profiles = await GetAllProfile();
		await PrintProfiles(printTable, profiles);
	}
	public static async Task PrintAllProfiles()
	{
		await PrintAllProfiles(
			printTable: Input.WriteToConsole.PrintTable);
	}
	public static async Task PrintTaskCountsByProfile(
	IEnumerable<Database.Profile> profiles)
	{
		await PrintTaskCountsByProfile(profiles: profiles,
			printTable: Input.WriteToConsole.PrintTable);
	}
	/// <summary>
	/// Asynchronously prints the task counts for all profiles using the specified table formatting action.
	/// </summary>
	/// <remarks>This method retrieves all profiles and prints their task counts by invoking the provided printTable
	/// action. The caller is responsible for supplying a suitable action to format and display the results.</remarks>
	/// <param name="printTable">An action that defines how to print the task counts. The action receives an array of profile names, an enumerable
	/// of task count arrays for each profile, and an optional string for additional formatting.</param>
	/// <returns>A task that represents the asynchronous print operation.</returns>
	public static async Task PrintTaskCountsByAllProfile(
	Func<string[], IEnumerable<string[]>, string?, Task> printTable)
	{
		IEnumerable<Database.Profile> profiles = await GetAllProfile();
		await PrintTaskCountsByProfile(printTable, profiles);
	}
	public static async Task PrintTaskCountsByAllProfile()
	{
		await PrintTaskCountsByAllProfile(printTable: Input.WriteToConsole.PrintTable);
	}
}

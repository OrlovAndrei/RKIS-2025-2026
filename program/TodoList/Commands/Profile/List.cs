using ShevricTodo.Authentication;

namespace ShevricTodo.Commands.Profile;

internal class List : Profile
{
	public static async System.Threading.Tasks.Task PrintAllProfiles(
	Action<string[], IEnumerable<string[]>, string?> printTable)
	{
		IEnumerable<Database.Profile> allProfile = await GetAllProfile();
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
			from profile in allProfile
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
	public static async System.Threading.Tasks.Task PrintTaskCountsByProfile(
	Action<string[], IEnumerable<string[]>, string?> printTable)
	{
		IEnumerable<(int profileId, int countTasks)> taskCountsByProfile = await GetTaskCountsByProfile();
		IEnumerable<Database.Profile> allProfile = await GetAllProfile();
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
			join profile in allProfile on profileCout.profileId equals profile.UserId
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
}

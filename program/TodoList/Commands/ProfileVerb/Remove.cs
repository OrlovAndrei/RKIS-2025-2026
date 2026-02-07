using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileVerb;

internal class Remove : Profile
{
	public static async Task<(int result, Database.Profile? deletedProfile)> Done(
		Func<Database.Profile, Task<IEnumerable<Database.Profile>>> searchProfile,
		Func<string, string> inputPassword,
		Func<Dictionary<int, string>, string?, int,
			KeyValuePair<int, string>> inputOneOf,
		Func<Database.Profile, Task> showProfile,
		Action<string> showMessage,
		Database.Profile searchTemplate)
	{
		IEnumerable<Database.Profile> profiles = await searchProfile(searchTemplate);
		int result = 0;
		switch (profiles.Count())
		{
			case 0:
				showMessage("Профиль не был найден.");
				break;
			case 1:
				await showProfile(profiles.First());
				if (await CheckPassword(inputPassword, profiles.First()))
				{
					using (Todo db = new())
					{
						db.Profiles.RemoveRange(profiles);
						result = await db.SaveChangesAsync();
						return (result, profiles.First());
					}
				}
				break;
			default:
				Database.Profile preciseProfile =
					await Search.Clarification(
						searchProfile: searchProfile,
						inputOneOf: inputOneOf,
						searchTemplate: searchTemplate,
						profiles: profiles);
				if (await CheckPassword(inputPassword, preciseProfile))
				{
					using (Todo db = new())
					{
						db.Profiles.RemoveRange(preciseProfile);
						result = await db.SaveChangesAsync();
						return (result, preciseProfile);
					}
				}
				break;
		}
		return (result, null);
	}
	public static async Task<(int result, Database.Profile? deletedProfile)> Done(
		Func<Database.Profile, Task<IEnumerable<Database.Profile>>> searchProfile,
		Database.Profile searchTemplate)
	{
		return await Done(searchProfile: searchProfile,
			inputPassword: Input.Password.GetPassword,
			inputOneOf: Input.OneOf.GetOneFromList,
			showMessage: Console.WriteLine,
			showProfile: Show.ShowProfile,
			searchTemplate: searchTemplate
			);
	}
	public static async Task<(int result, Database.Profile? deletedProfile)> DoneContains(
		Database.Profile searchTemplate)
	{
		return await Done(searchProfile: Search.SearchProfilesContains,
			searchTemplate: searchTemplate
			);
	}
	public static async Task<(int result, Database.Profile? deletedProfile)> DoneStartsWith(
		Database.Profile searchTemplate)
	{
		return await Done(searchProfile: Search.SearchProfilesStartsWith,
			searchTemplate: searchTemplate
			);
	}
	public static async Task<(int result, Database.Profile? deletedProfile)> DoneEndsWith(
		Database.Profile searchTemplate)
	{
		return await Done(searchProfile: Search.SearchProfilesEndsWith,
			searchTemplate: searchTemplate
			);
	}
}
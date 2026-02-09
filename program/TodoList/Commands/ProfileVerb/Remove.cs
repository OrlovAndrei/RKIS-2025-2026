using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileVerb;

internal class Remove : ProfileObj
{
	private static async Task<(int result, Profile? deletedProfile)> Done(
		Func<Profile, Task<IEnumerable<Profile>>> searchProfile,
		Func<string, string> inputPassword,
		Func<Dictionary<int, string>, string?, int,
			KeyValuePair<int, string>> inputOneOf,
		Func<Profile, Task> showProfile,
		Action<string> showMessage,
		Profile searchTemplate)
	{
		IEnumerable<Profile> profiles = await searchProfile(searchTemplate);
		Profile? preciseProfile = null;
		int result = 0;
		switch (profiles.Count())
		{
			case 0:
				showMessage("Профиль не был найден.");
				break;
			case 1:
				preciseProfile = profiles.First();
				await showProfile(preciseProfile);
				if (await CheckPassword(inputPassword, preciseProfile))
				{
					using (Todo db = new())
					{
						db.Profiles.RemoveRange(profiles);
						result = await db.SaveChangesAsync();
					}
				}
				break;
			default:
				preciseProfile =
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
					}
				}
				break;
		}
		return (result, preciseProfile);
	}
	private static async Task<(int result, Profile? deletedProfile)> Done(
		Func<Profile, Task<IEnumerable<Profile>>> searchProfile,
		Profile searchTemplate)
	{
		return await Done(searchProfile: searchProfile,
			inputPassword: Input.Password.GetPassword,
			inputOneOf: Input.OneOf.GetOneFromList,
			showMessage: Console.WriteLine,
			showProfile: Show.ShowProfile,
			searchTemplate: searchTemplate
			);
	}
	public static async Task<(int result, Profile? deletedProfile)> DoneContains(
		Profile searchTemplate)
	{
		return await Done(searchProfile: Search.SearchProfilesContains,
			searchTemplate: searchTemplate
			);
	}
	public static async Task<(int result, Profile? deletedProfile)> DoneStartsWith(
		Profile searchTemplate)
	{
		return await Done(searchProfile: Search.SearchProfilesStartsWith,
			searchTemplate: searchTemplate
			);
	}
	public static async Task<(int result, Profile? deletedProfile)> DoneEndsWith(
		Profile searchTemplate)
	{
		return await Done(searchProfile: Search.SearchProfilesEndsWith,
			searchTemplate: searchTemplate
			);
	}
}
using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileObj;

internal partial class Search : ProfileObj
{
	protected internal static async Task<IEnumerable<Profile>> SearchProfilesContains(
		Profile searchTemplate)
	{
		using (Todo db = new())
		{
			return await db.Profiles
				.StartFilter()
				.FilterTasksContainsAsync(searchTemplate: searchTemplate)
				.FilterIdEqualsAsync(searchTemplate: searchTemplate)
				.FilterDateEqualsAsync(searchTemplate: searchTemplate)
				.FinishFilter();
		}
	}
	protected internal static async Task<IEnumerable<Profile>> SearchProfilesStartsWith(
		Profile searchTemplate)
	{
		using (Todo db = new())
		{
			return await db.Profiles
				.StartFilter()
				.FilterTasksStartsWithAsync(searchTemplate: searchTemplate)
				.FilterIdEqualsAsync(searchTemplate: searchTemplate)
				.FilterDateEqualsAsync(searchTemplate: searchTemplate)
				.FinishFilter();
		}
	}
	protected internal static async Task<IEnumerable<Profile>> SearchProfilesEndsWith(
		Profile searchTemplate)
	{
		using (Todo db = new())
		{
			return await db.Profiles
				.StartFilter()
				.FilterTasksEndsWithAsync(searchTemplate: searchTemplate)
				.FilterIdEqualsAsync(searchTemplate: searchTemplate)
				.FilterDateEqualsAsync(searchTemplate: searchTemplate)
				.FinishFilter();
		}
	}
	private static async Task SearchAndPrintProfiles(
		Func<Profile, Task<IEnumerable<Profile>>> searchProfile,
		Action<string> showMessage,
		Func<Profile, Task> showProfile,
		Func<IEnumerable<Profile>, Task> showProfiles,
		Profile searchTemplate)
	{
		IEnumerable<Profile> profiles = await searchProfile(searchTemplate);
		switch (profiles.Count())
		{
			case 0:
				showMessage("Нет ни одного похожего профиля.");
				break;
			case 1:
				await showProfile(profiles.First());
				break;
			default:
				await showProfiles(profiles);
				break;
		}
	}
	protected internal static async Task<Profile> Clarification(
		Func<Profile, Task<IEnumerable<Profile>>> searchProfile,
		Func<Dictionary<int, string>,
			string?,
			int,
			KeyValuePair<int, string>> inputOneOf,
		Profile searchTemplate,
		IEnumerable<Profile> profiles)
	{
		KeyValuePair<int, string> profileIdAndName =
		inputOneOf(
			profiles.ToDictionary(p => p.UserId ?? -1, p => p.FirstName.NotAvailable()),
			"Выберите профиль:", 5);
		searchTemplate.UserId = profileIdAndName.Key;
		searchTemplate.FirstName = profileIdAndName.Value;
		return (await searchProfile(searchTemplate)).First();
	}
}

using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileObj;

internal partial class Change
{
	private static async Task<(int result, Profile? replacedProfile)> ProfileChange(
	Func<Profile, Task<IEnumerable<Profile>>> searchProfile,
	Profile searchTemplate)
	{
		return await ProfileChange(
			searchProfile: searchProfile,
			searchTemplate: searchTemplate,
			inputPassword: Input.Password.GetPassword,
			inputOneOf: Input.OneOf.GetOneFromList,
			showProfile: Show.ShowProfile,
			showMessage: Console.WriteLine);
	}
	public static async Task<(int result, Profile? replacedProfile)> ProfileContainsChange(
		Profile searchTemplate)
	{
		return await ProfileChange(
			searchProfile: Search.SearchProfilesContains,
			searchTemplate: searchTemplate);
	}
	public static async Task<(int result, Profile? replacedProfile)> ProfileStartsWithChange(
		Profile searchTemplate)
	{
		return await ProfileChange(
			searchProfile: Search.SearchProfilesStartsWith,
			searchTemplate: searchTemplate);
	}
	public static async Task<(int result, Profile? replacedProfile)> ProfileEndsWithChange(
		Profile searchTemplate)
	{
		return await ProfileChange(
			searchProfile: Search.SearchProfilesEndsWith,
			searchTemplate: searchTemplate);
	}
}

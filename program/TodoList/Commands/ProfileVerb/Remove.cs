using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileObj;

internal partial class Remove
{
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
using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileObj;

internal partial class Edit
{
	private static async Task<(int result, Profile? deletedProfile)> Done(
		Func<Profile, Task<IEnumerable<Profile>>> searchProfile,
		Profile searchTemplate,
		Profile updateTemplate) => await Done(
			searchProfile: searchProfile,
			inputPassword: Input.Password.GetPassword,
			inputOneOf: Input.OneOf.GetOneFromList,
			showProfile: Show.ShowProfile,
			showMessage: Console.WriteLine,
			searchTemplate: searchTemplate,
			updateTemplate: updateTemplate);
	public static async Task<(int result, Profile? deletedProfile)> DoneContains(
		Profile searchTemplate,
		Profile updateTemplate) => await Done(
			searchProfile: Search.SearchProfilesContains,
			searchTemplate: searchTemplate,
			updateTemplate: updateTemplate);
	public static async Task<(int result, Profile? deletedProfile)> DoneStartsWith(
		Profile searchTemplate,
		Profile updateTemplate) => await Done(
			searchProfile: Search.SearchProfilesStartsWith,
			searchTemplate: searchTemplate,
			updateTemplate: updateTemplate);
	public static async Task<(int result, Profile? deletedProfile)> DoneEndsWith(
		Profile searchTemplate,
		Profile updateTemplate) => await Done(
			searchProfile: Search.SearchProfilesEndsWith,
			searchTemplate: searchTemplate,
			updateTemplate: updateTemplate);
}
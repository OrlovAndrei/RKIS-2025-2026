using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileObj;

internal partial class Search
{
	private static async Task SearchAndPrintProfiles(
	Func<Profile, Task<IEnumerable<Profile>>> searchProfile,
	Profile searchTemplate) => await SearchAndPrintProfiles(searchProfile: searchProfile,
			showMessage: Console.WriteLine,
			showProfile: Show.ShowProfile,
			showProfiles: List.PrintProfiles,
			searchTemplate: searchTemplate);
	public static async Task SearchContainsAndPrintProfiles(
		Profile searchTemplate) => await SearchAndPrintProfiles(searchProfile: SearchProfilesContains,
			searchTemplate: searchTemplate);
	public static async Task SearchStartsWithAndPrintProfiles(
		Profile searchTemplate) => await SearchAndPrintProfiles(searchProfile: SearchProfilesStartsWith,
			searchTemplate: searchTemplate);
	public static async Task SearchEndsWithAndPrintProfiles(
		Profile searchTemplate) => await SearchAndPrintProfiles(searchProfile: SearchProfilesEndsWith,
			searchTemplate: searchTemplate);
}

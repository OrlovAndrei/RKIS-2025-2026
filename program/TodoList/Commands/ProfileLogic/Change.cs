using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileObj;

internal partial class Change : ProfileObj
{
	private static async Task<(int result, Profile? replacedProfile)> ProfileChange(
		Func<Profile, Task<IEnumerable<Profile>>> searchProfile,
		Func<string, string> inputPassword,
		Func<Dictionary<int, string>, string?, int,
			KeyValuePair<int, string>> inputOneOf,
		Action<string> showMessage,
		Func<Profile, Task> showProfile,
		Profile searchTemplate)
	{
		Profile active = await ActiveProfile.GetActiveProfile();
		IEnumerable<Profile> profiles = await searchProfile(searchTemplate);
		int result = 0;
		Profile preciseProfile;
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
					await ActiveProfile.Update(preciseProfile);
					return (++result, preciseProfile);
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
					await ActiveProfile.Update(preciseProfile);
					return (++result, preciseProfile);
				}
				break;
		}
		return (result, null);
	}
}

using ShevricTodo.Authentication;
using ShevricTodo.Database;
using TodoList.Commands;

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
		Profile? preciseProfile = await Logic<Profile>.ProcessQuantity(
			items: profiles,
			ifTheQuantityIsZero: ProfileNotFound,
			ifTheQuantityIsOne: ProfileIsOne,
			ifTheQuantityIsMany: ProfilesIsMore
		);
		int result = preciseProfile is not null ? 1 : 0;
		return (result, preciseProfile);

		async Task<Profile?> ProfileNotFound()
		{
			showMessage("Профиль не был найден.");
			return null;
		}
		async Task<Profile?> ProfileIsOne()
		{
			Profile preciseProfile = profiles.First();
			await showProfile(preciseProfile);
			if (await CheckPassword(inputPassword, preciseProfile))
			{
				await ActiveProfile.Update(preciseProfile);
				return preciseProfile;
			}
			return null;
		}
		async Task<Profile?> ProfilesIsMore()
		{
			Profile preciseProfile =
					await Search.Clarification(
						searchProfile: searchProfile,
						inputOneOf: inputOneOf,
						searchTemplate: searchTemplate,
						profiles: profiles);
			if (await CheckPassword(inputPassword, preciseProfile))
			{
				await ActiveProfile.Update(preciseProfile);
				return preciseProfile;
			}
			return null;
		}
	}
}

using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileObj;

public static class EnteringValues
{
	internal static async Task<Profile> EnteringDateOfCreate(
		this Profile profile)
	{
		profile.DateOfCreate = DateTime.Now;
		return profile;
	}
	internal static async Task<Profile> EnteringNewPassword(
		this Profile profile, Func<string> inputPassword)
	{
		profile.HashPassword = await Encryption.CreatePasswordHash(inputPassword(), profile.DateOfCreate);
		return profile;
	}
	internal static async Task<Profile> EnteringFirstName(
		this Profile profile,
		string message,
		Func<string, string?> inputString)
	{
		profile.FirstName ??= inputString(message);
		return profile;
	}
	internal static async Task<Profile> EnteringLastName(
		this Profile profile,
		string message,
		Func<string, string?> inputString)
	{
		profile.LastName ??= inputString(message);
		return profile;
	}
	internal static async Task<Profile> EnteringUserName(
		this Profile profile,
		Func<string, bool> inputBool,
		string messageQuestion,
		string message,
		Func<string, string?> inputString)
	{
		if (profile.UserName is null && inputBool(messageQuestion))
		{
			profile.UserName = inputString(message);
		}
		return profile;
	}
	internal static async Task<Profile> EnteringBirthday(
		this Profile profile,
		string message,
		Func<string, DateTime?> inputDateTime)
	{
		profile.Birthday ??= inputDateTime(message);
		return profile;
	}
}
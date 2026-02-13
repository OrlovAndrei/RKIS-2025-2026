using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileObj;

public static class EnteringValues
{
	internal static async Task<Profile> EnteringFirstName(
		this Profile profile,
		Func<string, string?> inputString)
	{
		profile.FirstName ??= inputString("Введите ваше имя: ");
		return profile;
	}
	internal static async Task<Profile> EnteringLastName(
		this Profile profile,
		Func<string, string?> inputString)
	{
		profile.LastName ??= inputString("Введите вашу фамилию: ");
		return profile;
	}
	internal static async Task<Profile> EnteringUserName(
		this Profile profile,
		Func<string, bool> inputBool,
		Func<string, string?> inputString)
	{
		if (profile.UserName is null && inputBool("Желаете ввести псевдоним? "))
		{
			profile.UserName = inputString("Введите ваш псевдоним: ");
		}
		return profile;
	}
	internal static async Task<Profile> EnteringBirthday(
		this Profile profile,
		Func<string, DateTime?> inputDateTime)
	{
		profile.Birthday ??= inputDateTime("Введите ваш день рождения: ");
		return profile;
	}
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
}
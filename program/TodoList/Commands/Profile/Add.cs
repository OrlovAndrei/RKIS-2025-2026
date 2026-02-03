using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.Profile;

internal class Add
{
	public static async Task<int> AddNewProfile(
		Database.Profile newProfile)
	{
		using (Todo db = new())
		{
			await db.Profiles.AddAsync(newProfile);
			return await db.SaveChangesAsync();
		}
	}
	public static async Task<int> AddNewProfile(
		Database.Profile[] newProfiles)
	{
		using (Todo db = new())
		{
			await db.Profiles.AddRangeAsync(newProfiles);
			return await db.SaveChangesAsync();
		}
	}
	public static async Task<(int resultSave, Database.Profile profile)> Done(
		Func<string, string?> inputString,
		Func<string, DateTime?> inputDateTime,
		Func<string, bool> inputBool,
		Func<string> inputPassword,
		string? firstName = null,
		string? lastName = null,
		string? userName = null,
		DateTime? birthday = null)
	{
		DateTime nowDateTime = DateTime.Now;
		Database.Profile newProfile = new()
		{
			FirstName = firstName ?? inputString("Введите ваше имя: "),
			LastName = lastName ?? inputString("Введите вашу фамилию: "),
			UserName = userName ?? (inputBool("Желаете ввести псевдоним? ")
				? inputString("Введите ваш псевдоним: ")
				: null),
			DateOfCreate = nowDateTime,
			Birthday = inputDateTime("Введите ваш день рождения: "),
			HashPassword = Encryption.CreateSHA256(inputPassword(),
				nowDateTime.ToShortDateString(),
				nowDateTime.ToShortTimeString())
		};
		return (await AddNewProfile(newProfile), newProfile);
	}
}

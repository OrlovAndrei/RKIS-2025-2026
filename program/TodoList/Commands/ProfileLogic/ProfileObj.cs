using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileObj;

internal partial class ProfileObj
{
	/// <summary>
	/// Добавляет новый профиль
	/// </summary>
	/// <param name="newProfile">Новый профиль</param>
	/// <returns>Количество изменений</returns>
	protected static async Task<int> AddNew(
		Database.Profile newProfile)
	{
		using (Todo db = new())
		{
			await db.Profiles.AddAsync(newProfile);
			return await db.SaveChangesAsync();
		}
	}
	/// <summary>
	/// Добавляет новый профиль
	/// </summary>
	/// <param name="newProfiles">Массив новых профилей</param>
	/// <returns>Количество изменений</returns>
	protected static async Task<int> AddNew(
		IEnumerable<Database.Profile> newProfiles)
	{
		using (Todo db = new())
		{
			await db.Profiles.AddRangeAsync(newProfiles);
			return await db.SaveChangesAsync();
		}
	}
	/// <summary>
	/// Asynchronously retrieves all user profiles from the database, ordered by user ID.
	/// </summary>
	/// <remarks>Ensure that the database context is properly configured and accessible before calling this method.
	/// The returned profiles are ordered by their associated user IDs.</remarks>
	/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
	/// cref="Database.Profile"/> objects, each representing a user profile.</returns>
	internal static async Task<IEnumerable<Database.Profile>> GetAllProfile()
	{
		using (Todo db = new())
		{
			return from profile in db.Profiles
				   orderby profile.UserId
				   select profile;
		}
	}
	/// <summary>
	/// Asynchronously retrieves a collection of user profile identifiers and the corresponding number of tasks associated
	/// with each profile.
	/// </summary>
	/// <remarks>This method queries the database to group tasks by user profile and count the number of tasks per
	/// profile. Ensure that the database context is properly configured and contains the necessary data for accurate
	/// results.</remarks>
	/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of tuples,
	/// where each tuple includes a profile ID and the count of tasks for that profile.</returns>
	protected static async Task<IEnumerable<(int Profile, int CountTasks)>> GetTaskCountsByProfile(
		IEnumerable<Database.Profile>? profiles = null)
	{
		using (Todo db = new())
		{
			profiles ??= await GetAllProfile();
			return (IEnumerable<(int ProfileId, int CountTasks)>)
				(from profile in profiles
				 join tasks in db.Tasks on profile.UserId equals tasks.UserId
				 group profile by profile.UserId into profileId
				 select new
				 {
					 ProfileId = profileId.Key,
					 CountTasks = profileId.Count()
				 });
		}
	}
	protected static async Task<bool> CheckPassword(
		Func<string, string> inputPassword,
		Database.Profile profile)
	{
		return profile.HashPassword ==
						await Encryption.CreatePasswordHash(
							inputPassword("Введите пароль: "),
							profile.DateOfCreate);
	}
}

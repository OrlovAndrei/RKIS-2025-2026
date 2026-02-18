using Microsoft.EntityFrameworkCore;

namespace ShevricTodo.Database;

public class Profile
{
	public int UserId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? UserName { get; set; }
	public DateTime? DateOfCreate { get; set; }
	public DateTime? Birthday { get; set; }
	public string? HashPassword { get; set; }
	public virtual ICollection<TaskTodo>? Tasks { get; set; }
	/// <summary>
	/// Добавляет новый профиль
	/// </summary>
	/// <param name="newProfile">Новый профиль</param>
	/// <returns>Количество изменений</returns>
	protected static async Task<int> Add(
		Profile newProfile)
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
	protected static async Task<int> Add(
		IEnumerable<Profile> newProfiles)
	{
		using (Todo db = new())
		{
			await db.Profiles.AddRangeAsync(newProfiles);
			return await db.SaveChangesAsync();
		}
	}
	protected internal static async Task<int> Remove(
		Profile profile)
	{
		using (Todo db = new())
		{
			db.Profiles.Remove(profile);
			return await db.SaveChangesAsync();
		}
	}
	protected internal static async Task<int> Remove(
		IEnumerable<Profile> profiles)
	{
		using (Todo db = new())
		{
			db.Profiles.RemoveRange(profiles);
			return await db.SaveChangesAsync();
		}
	}
	protected internal static async Task<int> Update(
		Profile profile)
	{
		using (Todo db = new())
		{
			db.Profiles.Update(profile);
			return await db.SaveChangesAsync();
		}
	}
	protected internal static async Task<int> Update(
		IEnumerable<Profile> profiles)
	{
		using (Todo db = new())
		{
			db.Profiles.UpdateRange(profiles);
			return await db.SaveChangesAsync();
		}
	}
	/// <summary>
	/// Asynchronously retrieves all user profiles from the database, ordered by user ID.
	/// </summary>
	/// <remarks>Ensure that the database context is properly configured and accessible before calling this method.
	/// The returned profiles are ordered by their associated user IDs.</remarks>
	/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
	/// cref="Profile"/> objects, each representing a user profile.</returns>
	internal static async Task<IEnumerable<Profile>> GetAllProfile()
	{
		using (Todo db = new())
		{
			return await
			(from profile in db.Profiles
			 orderby profile.UserId
			 select profile).ToArrayAsync();
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
		IEnumerable<Profile>? profiles = null)
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
	/// <summary>
	/// Asynchronously retrieves all tasks associated with the specified user profile.
	/// </summary>
	/// <remarks>This method queries the database for tasks linked to the user ID of the provided profile. Ensure
	/// that the profile is valid to avoid exceptions.</remarks>
	/// <param name="profile">The user profile for which to retrieve tasks. This parameter must not be null and should represent a valid profile
	/// with a valid user ID.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of TaskTodo
	/// objects that belong to the specified user profile. The collection is empty if no tasks are found.</returns>
	protected internal static async Task<IEnumerable<TaskTodo>> GetAllTasksOfProfile(
		Profile profile)
	{
		using (Todo db = new())
		{
			return await db.Tasks.Where(t => t.UserId == profile.UserId).ToArrayAsync();
		}
	}
}

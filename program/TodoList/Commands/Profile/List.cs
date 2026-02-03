using ShevricTodo.Database;

namespace ShevricTodo.Commands.Profile;

internal class List
{
	public static async Task<IEnumerable<Database.Profile>> GetAllProfile()
	{
		using (Todo db = new())
		{
			return from profile in db.Profiles
				   orderby profile.UserId
				   select profile;
		}
	}
	public static async Task<IEnumerable<(int ProfileId, int CountTasks)>> GetCountTaskProfile()
	{
		using (Todo db = new())
		{
			return (IEnumerable<(int ProfileId, int CountTasks)>)
				(from profile in db.Profiles
				 join tasks in db.Tasks on profile.UserId equals tasks.UserId
				 group profile by profile.UserId into profileId
				 select new
				 {
					 ProfileId = profileId.Key,
					 CountTasks = profileId.Count()
				 });
		}
	}
}

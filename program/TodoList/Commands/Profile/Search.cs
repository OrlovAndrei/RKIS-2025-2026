using Microsoft.EntityFrameworkCore;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.Profile;

internal class Search : Profile
{
	public static async Task<IEnumerable<Database.Profile>> SearchProfiles(
		Database.Profile searchTemplate)
	{
		using (Todo db = new())
		{
			var query = db.Profiles.AsQueryable();
			if (searchTemplate.UserId.HasValue)
			{
				query = query.Where(p => p.UserId == searchTemplate.UserId);
			}
			if (!string.IsNullOrEmpty(searchTemplate.FirstName))
			{
				query = query.Where(p => p.FirstName != null && p.FirstName.Contains(searchTemplate.FirstName));
			}
			if (!string.IsNullOrEmpty(searchTemplate.LastName))
			{
				query = query.Where(p => p.LastName != null && p.LastName.Contains(searchTemplate.LastName));
			}
			if (!string.IsNullOrEmpty(searchTemplate.UserName))
			{
				query = query.Where(p => p.UserName != null && p.UserName.Contains(searchTemplate.UserName));
			}
			if (searchTemplate.DateOfCreate.HasValue)
			{
				query = query.Where(p => p.DateOfCreate == searchTemplate.DateOfCreate);
			}
			if (searchTemplate.Birthday.HasValue)
			{
				query = query.Where(p => p.Birthday == searchTemplate.Birthday);
			}
			return await query.ToListAsync();
		}
	}
	public static async System.Threading.Tasks.Task SearchAndPrintProfiles(
		Func<Database.Profile, Task<IEnumerable<Database.Profile>>> searchProfile,
		Action<string> showMessage,
		Action<Database.Profile> showProfile,
		Action<IEnumerable<Database.Profile>> showProfiles,
		Database.Profile searchTemplate)
	{
		IEnumerable<Database.Profile> profiles = await searchProfile(searchTemplate);
		switch (profiles.Count())
		{
			case 0:
				showMessage("Нет ни одного похожего профиля.");
				break;
			case 1:
				showProfile(profiles.First());
				break;
			default:
				showProfiles(profiles);
				break;
		}
	}
}

using Microsoft.EntityFrameworkCore;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileVerb;

internal class Search : ProfileObj
{
	private static async Task<IQueryable<Profile>> FilterIdAndDate(
		IQueryable<Profile> query,
		Profile searchTemplate)
	{
		if (searchTemplate.UserId.HasValue)
		{
			query = query.Where(p => p.UserId == searchTemplate.UserId);
		}
		if (searchTemplate.DateOfCreate.HasValue)
		{
			query = query.Where(p => p.DateOfCreate == searchTemplate.DateOfCreate);
		}
		if (searchTemplate.Birthday.HasValue)
		{
			query = query.Where(p => p.Birthday == searchTemplate.Birthday);
		}
		return query;
	}
	public static async Task<IEnumerable<Profile>> SearchProfilesContains(
		Profile searchTemplate)
	{
		using (Todo db = new())
		{
			IQueryable<Profile> query = db.Profiles.AsQueryable();
			query = await FilterIdAndDate(query, searchTemplate);
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
			return await query.ToListAsync();
		}
	}
	public static async Task<IEnumerable<Profile>> SearchProfilesStartsWith(
		Profile searchTemplate)
	{
		using (Todo db = new())
		{
			IQueryable<Profile> query = db.Profiles.AsQueryable();
			query = await FilterIdAndDate(query, searchTemplate);
			if (!string.IsNullOrEmpty(searchTemplate.FirstName))
			{
				query = query.Where(p => p.FirstName != null && p.FirstName.StartsWith(searchTemplate.FirstName));
			}
			if (!string.IsNullOrEmpty(searchTemplate.LastName))
			{
				query = query.Where(p => p.LastName != null && p.LastName.StartsWith(searchTemplate.LastName));
			}
			if (!string.IsNullOrEmpty(searchTemplate.UserName))
			{
				query = query.Where(p => p.UserName != null && p.UserName.StartsWith(searchTemplate.UserName));
			}
			return await query.ToListAsync();
		}
	}
	public static async Task<IEnumerable<Profile>> SearchProfilesEndsWith(
		Profile searchTemplate)
	{
		using (Todo db = new())
		{
			IQueryable<Profile> query = db.Profiles.AsQueryable();
			query = await FilterIdAndDate(query, searchTemplate);
			if (!string.IsNullOrEmpty(searchTemplate.FirstName))
			{
				query = query.Where(p => p.FirstName != null && p.FirstName.EndsWith(searchTemplate.FirstName));
			}
			if (!string.IsNullOrEmpty(searchTemplate.LastName))
			{
				query = query.Where(p => p.LastName != null && p.LastName.EndsWith(searchTemplate.LastName));
			}
			if (!string.IsNullOrEmpty(searchTemplate.UserName))
			{
				query = query.Where(p => p.UserName != null && p.UserName.EndsWith(searchTemplate.UserName));
			}
			return await query.ToListAsync();
		}
	}
	public static async Task SearchAndPrintProfiles(
		Func<Profile, Task<IEnumerable<Profile>>> searchProfile,
		Action<string> showMessage,
		Func<Profile, Task> showProfile,
		Func<IEnumerable<Profile>, Task> showProfiles,
		Profile searchTemplate)
	{
		IEnumerable<Database.Profile> profiles = await searchProfile(searchTemplate);
		switch (profiles.Count())
		{
			case 0:
				showMessage("Нет ни одного похожего профиля.");
				break;
			case 1:
				await showProfile(profiles.First());
				break;
			default:
				await showProfiles(profiles);
				break;
		}
	}
	public static async Task SearchAndPrintProfiles(
		Func<Profile, Task<IEnumerable<Profile>>> searchProfile,
		Profile searchTemplate)
	{
		await SearchAndPrintProfiles(searchProfile: searchProfile,
			showMessage: Console.WriteLine,
			showProfile: Show.ShowProfile,
			showProfiles: List.PrintProfiles,
			searchTemplate: searchTemplate);
	}
	public static async Task SearchContainsAndPrintProfiles(
		Profile searchTemplate)
	{
		await SearchAndPrintProfiles(searchProfile: SearchProfilesContains,
			searchTemplate: searchTemplate);
	}
	public static async Task SearchStartsWithAndPrintProfiles(
		Profile searchTemplate)
	{
		await SearchAndPrintProfiles(searchProfile: SearchProfilesStartsWith,
			searchTemplate: searchTemplate);
	}
	public static async Task SearchEndsWithAndPrintProfiles(
		Profile searchTemplate)
	{
		await SearchAndPrintProfiles(searchProfile: SearchProfilesEndsWith,
			searchTemplate: searchTemplate);
	}
	public static async Task<Profile> Clarification(
		Func<Profile, Task<IEnumerable<Profile>>> searchProfile,
		Func<Dictionary<int, string>,
			string?,
			int,
			KeyValuePair<int, string>> inputOneOf,
		Profile searchTemplate,
		IEnumerable<Profile> profiles)
	{
		KeyValuePair<int, string> profileIdAndName = inputOneOf((Dictionary<int, string>)
					(from profile in profiles
					 select new { profile.UserId, profile.FirstName }),
					 "Какой профиль вы хотите удалить?", 5);
		searchTemplate.UserId = profileIdAndName.Key;
		searchTemplate.FirstName = profileIdAndName.Value;
		return (await searchProfile(searchTemplate)).First();
	}
}

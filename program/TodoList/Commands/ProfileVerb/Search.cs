using Microsoft.EntityFrameworkCore;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.ProfileVerb;

internal class Search : Profile
{
	private static async Task<IQueryable<Database.Profile>> FilterIdAndDate(
		IQueryable<Database.Profile> query,
		Database.Profile searchTemplate)
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
	public static async Task<IEnumerable<Database.Profile>> SearchProfilesContains(
		Database.Profile searchTemplate)
	{
		using (Todo db = new())
		{
			IQueryable<Database.Profile> query = db.Profiles.AsQueryable();
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
	public static async Task<IEnumerable<Database.Profile>> SearchProfilesStartsWith(
		Database.Profile searchTemplate)
	{
		using (Todo db = new())
		{
			IQueryable<Database.Profile> query = db.Profiles.AsQueryable();
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
	public static async Task<IEnumerable<Database.Profile>> SearchProfilesEndsWith(
		Database.Profile searchTemplate)
	{
		using (Todo db = new())
		{
			IQueryable<Database.Profile> query = db.Profiles.AsQueryable();
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
		Func<Database.Profile, Task<IEnumerable<Database.Profile>>> searchProfile,
		Action<string> showMessage,
		Func<Database.Profile, Task> showProfile,
		Func<IEnumerable<Database.Profile>, Task> showProfiles,
		Database.Profile searchTemplate)
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
		Func<Database.Profile, Task<IEnumerable<Database.Profile>>> searchProfile,
		Database.Profile searchTemplate)
	{
		await SearchAndPrintProfiles(searchProfile: searchProfile,
			showMessage: Console.WriteLine,
			showProfile: Show.ShowProfile,
			showProfiles: List.PrintProfiles,
			searchTemplate: searchTemplate);
	}
	public static async Task SearchContainsAndPrintProfiles(
		Database.Profile searchTemplate)
	{
		await SearchAndPrintProfiles(searchProfile: SearchProfilesContains,
			searchTemplate: searchTemplate);
	}
	public static async Task SearchStartsWithAndPrintProfiles(
		Database.Profile searchTemplate)
	{
		await SearchAndPrintProfiles(searchProfile: SearchProfilesStartsWith,
			searchTemplate: searchTemplate);
	}
	public static async Task SearchEndsWithAndPrintProfiles(
		Database.Profile searchTemplate)
	{
		await SearchAndPrintProfiles(searchProfile: SearchProfilesEndsWith,
			searchTemplate: searchTemplate);
	}
	public static async Task<Database.Profile> Clarification(
		Func<Database.Profile, Task<IEnumerable<Database.Profile>>> searchProfile,
		Func<Dictionary<int, string>,
			string?,
			int,
			KeyValuePair<int, string>> inputOneOf,
		Database.Profile searchTemplate,
		IEnumerable<Database.Profile> profiles)
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

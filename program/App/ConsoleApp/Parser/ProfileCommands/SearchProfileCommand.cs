using Application.Dto;
using Application.Specifications.Criteria;
using Application.UseCase.ProfileUseCases.Query;
using ConsoleApp.Output.Implementation;
using ConsoleApp.Parser.Verb;

namespace ConsoleApp.Parser.ProfileCommands;

internal static class SearchProfileCommand
{
    internal static async Task Execute(ProfileSearch p)
    {
        // Determine search type
        var searchType = SearchTypes.Contains;
		if (!string.IsNullOrWhiteSpace(p.SearchType) && Enum.TryParse<SearchTypes>(p.SearchType, true, out var parsedType))
		{
			searchType = parsedType;
		}

		var searchDto = new ProfileDto.ProfileSearchDto(
			FirstName: p.FirstName,
			LastName: p.LastName,
			CreatedAtFrom: p.CreateAtFrom,
			CreatedAtTo: p.CreateAtTo,
			DateOfBirthFrom: p.DateOfBirthFrom,
			DateOfBirthTo: p.DateOfBirthTo,
			SearchType: searchType);

        var repo = Launch.ProfileRepository;
        var useCase = new FindProfilesUseCase(repository: repo, searchDto: searchDto);
        var res = await useCase.Execute();
        
        // Apply top limit if specified
        if (p.Top.HasValue && p.Top > 0)
        {
            res = res.Take(p.Top.Value);
        }
        
        PrintProfiles(res);
    }

    private static void PrintProfiles(IEnumerable<ProfileDto.ProfileDetailsDto> profiles)
    {
        var cols = new[] { "Id", "First", "Last", "Birth", "Created" };
        var rows = profiles.Select(p => new[]
        {
            p.ProfileId.ToString(),
            p.FirstName,
            p.LastName,
            p.DateOfBirth.ToShortDateString(),
            p.CreatedAt.ToString()
        });
        WriteToConsole.PrintTable(cols, rows);
    }
}

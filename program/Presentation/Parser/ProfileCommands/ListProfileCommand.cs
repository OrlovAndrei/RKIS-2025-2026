using Application.Dto;
using Application.UseCase.ProfileUseCases.Query;
using Presentation.Output.Implementation;
using Presentation.Parser.Verb;

namespace Presentation.Parser.ProfileCommands;

internal static class ListProfileCommand
{
    internal static async Task Execute(ProfileList p)
    {
        var repo = Launch.ProfileRepository;
        var useCase = new GetAllProfilesUseCase(repository: repo);
        var profiles = await useCase.Execute();
        
        // Apply top limit if specified
        if (p.Top.HasValue && p.Top > 0)
        {
            profiles = profiles.Take(p.Top.Value);
        }
        
        PrintProfiles(profiles);
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

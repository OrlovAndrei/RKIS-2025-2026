using Application.Dto;
using Application.Specifications.Criteria;
using Application.UseCase.ProfileUseCases;
using Application.UseCase.ProfileUseCases.Query;
using ConsoleApp.Adapters;
using ConsoleApp.Output.Implementation;
using ConsoleApp.Parser.Verb;
using static System.Console;

namespace ConsoleApp.Parser.ProfileCommands;

internal static class ChangeProfileCommand
{
    private static readonly InputAdapter _input = new();

    internal static async Task Execute(ProfileChange p)
    {
        var repo = Launch.ProfileRepository;
        var userContext = Launch.UserContext;
        var hasher = Launch.PasswordHasher;
        var commandManager = Launch.CommandManager;

        // Request search criteria
        var firstName = string.IsNullOrWhiteSpace(p.FirstNameSearch)
            ? _input.GetShortText("Введите имя для поиска (или пропустите): ", false)
            : p.FirstNameSearch;

        var lastName = string.IsNullOrWhiteSpace(p.LastNameSearch)
            ? _input.GetShortText("Введите фамилию для поиска (или пропустите): ", false)
            : p.LastNameSearch;

        // Search profiles by criteria
        var searchDto = new ProfileDto.ProfileSearchDto(
            FirstName: string.IsNullOrWhiteSpace(firstName) ? null : firstName,
            LastName: string.IsNullOrWhiteSpace(lastName) ? null : lastName,
            SearchType: SearchTypes.Contains,
            DateOfBirthFrom: p.DateOfBirthFromSearch,
            DateOfBirthTo: p.DateOfBirthToSearch,
            CreatedAtFrom: p.CreateAtFromSearch,
            CreatedAtTo: p.CreateAtToSearch
        );

        var findUseCase = new FindProfilesUseCase(repository: repo, searchDto: searchDto);
        var foundProfiles = (await findUseCase.Execute()).ToList();
        ProfileDto.ProfileDetailsDto selectedProfile;

        if (foundProfiles.Count == 0)
        {
            WriteToConsole.ColorMessage("Профили не найдены.", ConsoleColor.Yellow);
            return;
        }
        else if (foundProfiles.Count == 1)
        {
            selectedProfile = foundProfiles[0];
        }
        else
        {
            // Display profiles with numbers
            WriteToConsole.ColorMessage("Доступные профили:", ConsoleColor.Cyan);
            for (int i = 0; i < foundProfiles.Count; i++)
            {
                WriteLine($"{i + 1}. {foundProfiles[i].FirstName} {foundProfiles[i].LastName}");
            }

            // Request profile selection
            var selectionStr = _input.GetShortText("Выберите номер профиля: ");
            if (!int.TryParse(selectionStr, out int selection) || selection < 1 || selection > foundProfiles.Count)
            {
                WriteToConsole.ColorMessage("Некорректный выбор.", ConsoleColor.Red);
                return;
            }

            selectedProfile = foundProfiles[selection - 1];
        }

        // Request password
        var password = string.IsNullOrWhiteSpace(p.Password)
            ? _input.GetPassword("Введите пароль: ")
            : p.Password;

        // Create and execute change profile use case
        var changeUseCase = new ChangeProfileUseCase(
            repository: repo,
            hashed: hasher,
            userContext: userContext,
            newProfile: selectedProfile.ProfileId,
            password: password
        );

        int result = await commandManager.ExecuteCommandAsync(changeUseCase);
        if (result == 1)
        {
            WriteToConsole.ColorMessage("Профиль успешно изменён.", ConsoleColor.Green);
        }
        else
        {
            WriteToConsole.ColorMessage("Неверный пароль.", ConsoleColor.Red);
        }
    }
}

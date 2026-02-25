using Application.Dto;
using Application.Specifications.Criteria;
using Application.UseCase.ProfileUseCases;
using Application.UseCase.ProfileUseCases.Query;
using Presentation.Adapters;
using Presentation.Output.Implementation;
using Presentation.Parser.Verb;
using static System.Console;

namespace Presentation.Parser.ProfileCommands;

internal static class RemoveProfileCommand
{
    private static readonly InputAdapter _input = new();

    internal static async Task<int> Execute(ProfileRemove p)
    {
        var repo = Launch.ProfileRepository;
        var hasher = Launch.PasswordHasher;
        var commandManager = Launch.CommandManager;
        int result = 0;

        // Request search criteria
        var firstName = string.IsNullOrWhiteSpace(p.FirstName)
            ? _input.GetShortText("Введите имя профиля для удаления (или пропустите): ", false)
            : p.FirstName;
        
        var lastName = string.IsNullOrWhiteSpace(p.LastName)
            ? _input.GetShortText("Введите фамилию профиля для удаления (или пропустите): ", false)
            : p.LastName;

        // Search profiles by criteria
        var searchDto = new ProfileDto.ProfileSearchDto(
			FirstName: string.IsNullOrWhiteSpace(firstName) ? null : firstName,
			LastName: string.IsNullOrWhiteSpace(lastName) ? null : lastName,
			CreatedAtFrom: p.CreateAtFrom,
			CreatedAtTo: p.CreateAtTo,
			DateOfBirthFrom: p.DateOfBirthFrom,
			DateOfBirthTo: p.DateOfBirthTo,
			SearchType: SearchTypes.Contains);

        var findUseCase = new FindProfilesUseCase(repository: repo, searchDto: searchDto);
        var foundProfiles = (await findUseCase.Execute()).ToList();

        if (foundProfiles.Count == 0)
        {
            WriteToConsole.ColorMessage("Профили не найдены.", ConsoleColor.Yellow);
            return result;
        }

        // Display profiles with numbers
        WriteToConsole.ColorMessage("Доступные профили:", ConsoleColor.Cyan);
        for (int i = 0; i < foundProfiles.Count; i++)
        {
            WriteLine($"{i + 1}. {foundProfiles[i].FirstName} {foundProfiles[i].LastName}");
        }

        // Request profile selection
        var selectionStr = _input.GetShortText("Выберите номер профиля для удаления: ");
        if (!int.TryParse(selectionStr, out int selection) || selection < 1 || selection > foundProfiles.Count)
        {
            WriteToConsole.ColorMessage("Некорректный выбор.", ConsoleColor.Red);
            return result;
        }

        var selectedProfile = foundProfiles[selection - 1];

        // Confirm deletion
        var confirm = _input.GetYesNoChoice("Вы уверены что хотите удалить этот профиль? (y/n): ");
        if (!confirm)
        {
            WriteToConsole.ColorMessage("Удаление отменено.", ConsoleColor.Yellow);
            return result;
        }

        // Request password
        var password = string.IsNullOrWhiteSpace(p.Password)
            ? _input.GetPassword("Введите пароль для подтверждения удаления: ")
            : p.Password;

        // Create and execute deletion profile use case
        var deleteUseCase = new DeletionProfileUseCase(
            repository: repo,
            hashed: hasher,
            idProfile: selectedProfile.ProfileId,
            password: password
        );

        try
        {
            result = await commandManager.ExecuteCommandAsync(deleteUseCase);
            if (result > 0)
            {
                WriteToConsole.ColorMessage("Профиль успешно удалён.", ConsoleColor.Green);
                return result;
            }
            else
            {
                WriteToConsole.ColorMessage("Ошибка при удалении профиля.", ConsoleColor.Red);
                return result;

            }
        }
        catch (ArgumentException ex)
        {
            WriteToConsole.ColorMessage($"Ошибка: {ex.Message}", ConsoleColor.Red);
        }
        return result;
    }
}

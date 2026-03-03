using Application.Dto;
using Application.Specifications.Criteria;
using Application.UseCase.ProfileUseCases;
using Application.UseCase.ProfileUseCases.Query;
using ConsoleApp.Adapters;
using ConsoleApp.Output.Implementation;
using ConsoleApp.Parser.Verb;
using static System.Console;

namespace ConsoleApp.Parser.ProfileCommands;

internal static class EditProfileCommand
{
    private static readonly InputAdapter _input = new();

    internal static async Task Execute(ProfileEdit p)
    {
        var repo = Launch.ProfileRepository;
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
			CreatedAtFrom: p.CreateAtFromSearch,
			CreatedAtTo: p.CreateAtToSearch,
			DateOfBirthFrom: p.DateOfBirthFromSearch,
			DateOfBirthTo: p.DateOfBirthToSearch,
			SearchType: SearchTypes.Contains);

        var findUseCase = new FindProfilesUseCase(repository: repo, searchDto: searchDto);
        var foundProfiles = (await findUseCase.Execute()).ToList();

        if (foundProfiles.Count == 0)
        {
            WriteToConsole.ColorMessage("Профили не найдены.", ConsoleColor.Yellow);
            return;
        }

        // Display profiles with numbers
        WriteToConsole.ColorMessage("Доступные профили:", ConsoleColor.Cyan);
        for (int i = 0; i < foundProfiles.Count; i++)
        {
            WriteLine($"{i + 1}. {foundProfiles[i].FirstName} {foundProfiles[i].LastName}");
        }

        // Request profile selection
        var selectionStr = _input.GetShortText("Выберите номер профиля для редактирования: ");
        if (!int.TryParse(selectionStr, out int selection) || selection < 1 || selection > foundProfiles.Count)
        {
            WriteToConsole.ColorMessage("Некорректный выбор.", ConsoleColor.Red);
            return;
        }

        var selectedProfile = foundProfiles[selection - 1];

        // Get updated information
        var newFirstName = string.IsNullOrWhiteSpace(p.FirstName)
            ? _input.GetShortText($"Введите новое имя (текущее: {selectedProfile.FirstName}) (или пропустите): ", false)
            : p.FirstName;
        
        var newLastName = string.IsNullOrWhiteSpace(p.LastName)
            ? _input.GetShortText($"Введите новую фамилию (текущее: {selectedProfile.LastName}) (или пропустите): ", false)
            : p.LastName;
        
        var newDobStr = p.DateOfBirth?.ToString("yyyy-MM-dd") ?? string.Empty;
        if (string.IsNullOrWhiteSpace(newDobStr))
        {
            newDobStr = _input.GetShortText($"Введите новую дату рождения (текущее: {selectedProfile.DateOfBirth:yyyy-MM-dd}) (yyyy-MM-dd или пропустите): ", false);
        }

        var updateFirstName = string.IsNullOrWhiteSpace(newFirstName) ? selectedProfile.FirstName : newFirstName;
        var updateLastName = string.IsNullOrWhiteSpace(newLastName) ? selectedProfile.LastName : newLastName;
        var updateDob = string.IsNullOrWhiteSpace(newDobStr) ? selectedProfile.DateOfBirth : Parse.ParseDate(newDobStr) ?? selectedProfile.DateOfBirth;

        // Request password
        var password = string.IsNullOrWhiteSpace(p.Password)
            ? _input.GetPassword("Введите пароль для подтверждения изменений: ")
            : p.Password;

        // Create update DTO
        var updateDto = new ProfileDto.ProfileUpdateDto(
            ProfileId: selectedProfile.ProfileId,
            FirstName: updateFirstName,
            LastName: updateLastName,
            DateOfBirth: updateDob
        );

        // Create and execute update profile use case
        var updateUseCase = new UpdateProfileUseCase(
            repository: repo,
            hashed: hasher,
            profileUpdate: updateDto,
            password: password
        );

        try
        {
            int result = await commandManager.ExecuteCommandAsync(updateUseCase);
            if (result > 0)
            {
                WriteToConsole.ColorMessage("Профиль успешно обновлен.", ConsoleColor.Green);
            }
            else
            {
                WriteToConsole.ColorMessage("Ошибка при обновлении профиля.", ConsoleColor.Red);
            }
        }
        catch (ArgumentException ex)
        {
            WriteToConsole.ColorMessage($"Ошибка: {ex.Message}", ConsoleColor.Red);
        }
    }
}

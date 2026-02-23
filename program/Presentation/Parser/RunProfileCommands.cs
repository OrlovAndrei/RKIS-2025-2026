using Application.Dto;
using Application.Specifications;
using Application.UseCase.ProfileUseCases;
using Presentation.Input;
using static System.Console;

namespace Presentation.Parser;

internal static class RunProfileCommands
{
    public async static Task Run(Verb.Profile p)
    {
        if (p is null) return;

        // Add profile
        if (p.Add)
        {
            var first = string.IsNullOrWhiteSpace(p.FirstName)
                ? Text.ShortText("Введите имя: ")
                : p.FirstName!;

            var last = string.IsNullOrWhiteSpace(p.LastName)
                ? Text.ShortText("Введите фамилию: ")
                : p.LastName!;

            DateTime dob = Parse.ParseDate(p.Birthday) ?? DateTime.MinValue;
            if (dob == DateTime.MinValue)
            {
                var dobStr = Text.ShortText("Введите дату рождения (yyyy-MM-dd): ", true);
                dob = Parse.ParseDate(dobStr) ?? DateTime.MinValue;
            }

            var password = Password.CheckingThePassword();

            var createDto = new ProfileDto.ProfileCreateDto(
                FirstName: first,
                LastName: last,
                DateOfBirth: dob,
                Password: password
            );

            var repo = Launch.ProfileRepository ?? throw new Exception("Profile repository not initialized. Call Launch.UpdateRepositories first.");
            var hasher = Launch.PasswordHasher ?? throw new Exception("Password hasher not initialized. Call Launch.UpdateRepositories first.");

            var useCase = new AddNewProfileUseCase(
                repository: repo,
                hashed: hasher,
                profileCreate: createDto
            );

            await useCase.Execute();
            return;
        }

        // List profiles
        if (p.List)
        {
            var repo = Launch.ProfileRepository ?? throw new Exception("Profile repository not initialized.");
            var useCase = new GetAllProfilesUseCase(repository: repo);
            var profiles = await useCase.Execute();
            PrintProfiles(profiles);
            return;
        }

        // Search profiles
        if (p.Search)
        {
            var searchType = p.StartWith ? SearchType.StartsWith : p.EndsWith ? SearchType.EndsWith : SearchType.Contains;
            var searchDto = new ProfileDto.ProfileSearchDto(
                FirstName: p.FirstName,
                LastName: p.LastName,
                SearchType: searchType
            );

            var repo = Launch.ProfileRepository ?? throw new Exception("Profile repository not initialized.");
            var useCase = new FindProfilesUseCase(repository: repo, searchDto: searchDto);
            var res = await useCase.Execute();
            PrintProfiles(res);
            return;
        }

        // Change profile
        if (p.Change)
        {
            var repo = Launch.ProfileRepository ?? throw new Exception("Profile repository not initialized.");
            var userContext = Launch.UserContext ?? throw new Exception("User context not initialized.");
            var hasher = Launch.PasswordHasher ?? throw new Exception("Password hasher not initialized.");

            // Request search criteria
            var firstName = Text.ShortText("Введите имя для поиска (или пропустите): ", true);
            var lastName = Text.ShortText("Введите фамилию для поиска (или пропустите): ", true);

            // Search profiles by criteria
            var searchDto = new ProfileDto.ProfileSearchDto(
                FirstName: string.IsNullOrWhiteSpace(firstName) ? null : firstName,
                LastName: string.IsNullOrWhiteSpace(lastName) ? null : lastName,
                SearchType: SearchType.Contains
            );

            var findUseCase = new FindProfilesUseCase(repository: repo, searchDto: searchDto);
            var foundProfiles = (await findUseCase.Execute()).ToList();

            if (!foundProfiles.Any())
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
            var selectionStr = Text.ShortText("Выберите номер профиля: ");
            if (!int.TryParse(selectionStr, out int selection) || selection < 1 || selection > foundProfiles.Count)
            {
                WriteToConsole.ColorMessage("Некорректный выбор.", ConsoleColor.Red);
                return;
            }

            var selectedProfile = foundProfiles[selection - 1];

            // Request password
            var password = Password.CheckingThePassword();

            // Create and execute change profile use case
            var changeUseCase = new ChangeProfileUseCase(
                repository: repo,
                hashed: hasher,
                userContext: userContext,
                newProfile: selectedProfile.ProfileId,
                password: password
            );

            var result = await changeUseCase.Execute();
            if (result == 1)
            {
                WriteToConsole.ColorMessage("Профиль успешно изменён.", ConsoleColor.Green);
            }
            else
            {
                WriteToConsole.ColorMessage("Неверный пароль.", ConsoleColor.Red);
            }
            return;
        }
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

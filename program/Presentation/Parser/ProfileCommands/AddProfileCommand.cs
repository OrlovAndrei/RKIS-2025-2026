using Application.Dto;
using Application.UseCase.ProfileUseCases;
using Presentation.Adapters;
using Presentation.Output.Implementation;
using Presentation.Parser.Verb;

namespace Presentation.Parser.ProfileCommands;

internal static class AddProfileCommand
{
    private static readonly InputAdapter _input = new();

    internal static async Task Execute(ProfileAdd p)
    {
        var first = string.IsNullOrWhiteSpace(p.FirstName)
            ? _input.GetShortText("Введите имя: ", true)
            : p.FirstName;

        var last = string.IsNullOrWhiteSpace(p.LastName)
            ? _input.GetShortText("Введите фамилию: ", true)
            : p.LastName;

        DateTime dob = p.DateOfBirth is not null 
            ? p.DateOfBirth.Value
            : DateTime.MinValue;
        
        if (dob == DateTime.MinValue)
        {
            var dobStr = _input.GetShortText("Введите дату рождения (yyyy-MM-dd): ");
            dob = Parse.ParseDate(dobStr) ?? DateTime.MinValue;
        }

        var password = string.IsNullOrWhiteSpace(p.Password)
            ? _input.GetCheckedPassword()
            : p.Password;

        var createDto = new ProfileDto.ProfileCreateDto(
            FirstName: first,
            LastName: last,
            DateOfBirth: dob,
            Password: password
        );

        var repo = Launch.ProfileRepository;
        var hasher = Launch.PasswordHasher;
        var commandManager = Launch.CommandManager;

        var useCase = new AddNewProfileUseCase(
            repository: repo,
            hashed: hasher,
            profileCreate: createDto
        );

        int result = await commandManager.ExecuteCommandAsync(useCase);
        if (result > 0)
        {
            WriteToConsole.ColorMessage("Профиль успешно добавлен.", ConsoleColor.Green);
        }
        else
        {
            WriteToConsole.ColorMessage("Ошибка при добавлении профиля.", ConsoleColor.Red);
        }
    }
}

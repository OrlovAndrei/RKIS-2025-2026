using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public sealed partial class RegisterViewModel : ObservableObject
{
    private readonly ProfileRepository _profileRepository;
    private readonly TodoRepository _todoRepository;
    private readonly NavigationService _navigation;
    private readonly ProfileSessionService _session;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _login = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _password = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _firstName = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _lastName = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _birthYear = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public RegisterViewModel(
        ProfileRepository profileRepository,
        TodoRepository todoRepository,
        NavigationService navigation,
        ProfileSessionService session)
    {
        _profileRepository = profileRepository;
        _todoRepository = todoRepository;
        _navigation = navigation;
        _session = session;
    }

    public bool CanRegister =>
        !string.IsNullOrWhiteSpace(Login) &&
        !string.IsNullOrWhiteSpace(Password) &&
        !string.IsNullOrWhiteSpace(FirstName) &&
        !string.IsNullOrWhiteSpace(LastName) &&
        !string.IsNullOrWhiteSpace(BirthYear);

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private void Register()
    {
        ErrorMessage = string.Empty;

        if (!int.TryParse(BirthYear, out int parsedBirthYear))
        {
            ErrorMessage = "Год рождения должен быть числом.";
            return;
        }

        int currentYear = DateTime.Now.Year;
        if (parsedBirthYear < 1900 || parsedBirthYear > currentYear)
        {
            ErrorMessage = $"Год рождения должен быть в диапазоне 1900-{currentYear}.";
            return;
        }

        try
        {
            var profile = new Profile(
                Login.Trim(),
                Password,
                FirstName.Trim(),
                LastName.Trim(),
                parsedBirthYear);

            _profileRepository.Add(profile);
            _session.CurrentProfile = profile;
            _navigation.Navigate(new TodoListViewModel(_profileRepository, _todoRepository, _navigation, _session));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void BackToLogin()
    {
        _navigation.Navigate(new LoginViewModel(_profileRepository, _todoRepository, _navigation, _session));
    }
}

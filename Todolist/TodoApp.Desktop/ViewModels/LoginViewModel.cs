using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public sealed partial class LoginViewModel : ObservableObject
{
    private readonly ProfileRepository _profileRepository;
    private readonly TodoRepository _todoRepository;
    private readonly NavigationService _navigation;
    private readonly ProfileSessionService _session;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SignInCommand))]
    private string _login = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SignInCommand))]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public LoginViewModel(
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

    public bool CanLogin => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private void SignIn()
    {
        Profile? profile = _profileRepository.GetByLogin(Login);
        if (profile == null || profile.Password != Password)
        {
            ErrorMessage = "Неверный логин или пароль.";
            return;
        }

        _session.CurrentProfile = profile;
        _navigation.Navigate(new TodoListViewModel(_profileRepository, _todoRepository, _navigation, _session));
    }

    [RelayCommand]
    private void OpenRegister()
    {
        _navigation.Navigate(new RegisterViewModel(_profileRepository, _todoRepository, _navigation, _session));
    }
}

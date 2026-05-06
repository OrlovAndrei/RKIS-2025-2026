using CommunityToolkit.Mvvm.ComponentModel;
using TodoApp.Data;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels;

public sealed partial class MainViewModel : ObservableObject
{
    private readonly NavigationService _navigation;
    private readonly ProfileRepository _profileRepository;
    private readonly TodoRepository _todoRepository;
    private readonly ProfileSessionService _session;

    [ObservableProperty]
    private object? _currentViewModel;

    public MainViewModel(
        NavigationService navigation,
        ProfileRepository profileRepository,
        TodoRepository todoRepository,
        ProfileSessionService session)
    {
        _navigation = navigation;
        _profileRepository = profileRepository;
        _todoRepository = todoRepository;
        _session = session;

        _navigation.Initialize(viewModel => CurrentViewModel = viewModel);
        NavigateToLogin();
    }

    private void NavigateToLogin()
    {
        _session.CurrentProfile = null;
        _navigation.Navigate(new LoginViewModel(_profileRepository, _todoRepository, _navigation, _session));
    }
}

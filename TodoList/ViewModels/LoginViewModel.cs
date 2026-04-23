using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private string _login = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        public LoginViewModel(ITodoRepository todoRepository, IProfileRepository profileRepository, INavigationService navigationService, IDialogService dialogService)
        {
            _todoRepository = todoRepository;
            _profileRepository = profileRepository;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                await _dialogService.ShowErrorAsync("Введите логин и пароль");
                return;
            }

            IsBusy = true;
            ErrorMessage = null;

            try
            {
                var profile = await _profileRepository.AuthenticateAsync(Login, Password);
                if (profile != null)
                {
                    await _profileRepository.SetCurrentProfileAsync(profile);
                    var todoVm = new TodoListViewModel(
                        _todoRepository,
                        _profileRepository,
                        _navigationService,
                        _dialogService);
                    _navigationService.NavigateTo(todoVm);
                }
                else
                {
                    ErrorMessage = "Неверный логин или пароль";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка входа: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void NavigateToRegister()
        {
            var registerVm = new RegisterViewModel(_todoRepository, _profileRepository, _navigationService, _dialogService);
            _navigationService.NavigateTo(registerVm);
        }
    }
}

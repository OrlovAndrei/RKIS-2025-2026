using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;
using TodoApp.Data;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels
{
    public partial class RegisterViewModel : ViewModelBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private string _login = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _confirmPassword = string.Empty;

        [ObservableProperty]
        private string _firstName = string.Empty;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private int _birthYear = 2000;

        public RegisterViewModel(ITodoRepository todoRepository, IProfileRepository profileRepository, INavigationService navigationService, IDialogService dialogService)
        {
            _todoRepository = todoRepository;
            _profileRepository = profileRepository;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                await _dialogService.ShowErrorAsync("Введите логин");
                return;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                await _dialogService.ShowErrorAsync("Введите пароль");
                return;
            }
            if (Password != ConfirmPassword)
            {
                await _dialogService.ShowErrorAsync("Пароли не совпадают");
                return;
            }
            if (BirthYear < 1900 || BirthYear > DateTime.Now.Year)
            {
                await _dialogService.ShowErrorAsync($"Год рождения должен быть между 1900 и {DateTime.Now.Year}");
                return;
            }

            IsBusy = true;
            ErrorMessage = null;

            try
            {
                var existing = await _profileRepository.GetByLoginAsync(Login);
                if (existing != null)
                {
                    ErrorMessage = "Пользователь с таким логином уже существует";
                    return;
                }

                var profile = new Profile(Login, Password, FirstName, LastName, BirthYear);
                await _profileRepository.AddAsync(profile);
                await _dialogService.ShowMessageAsync("Регистрация прошла успешно! Теперь вы можете войти.");
                var loginVm = new LoginViewModel(_todoRepository, _profileRepository, _navigationService, _dialogService);
                _navigationService.NavigateTo(loginVm);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка регистрации: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void NavigateToLogin()
        {
            var loginVm = new LoginViewModel(_todoRepository, _profileRepository, _navigationService, _dialogService);
            _navigationService.NavigateTo(loginVm);
        }
    }
}

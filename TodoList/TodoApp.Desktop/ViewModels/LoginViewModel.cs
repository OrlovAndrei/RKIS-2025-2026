using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ProfileRepository _profileRepo = new();

        [ObservableProperty]
        private string login = "";

        [ObservableProperty]
        private string password = "";

        [ObservableProperty]
        private string errorMessage = "";

        private INavigationService? Navigation =>
            (App.Current.MainWindow.DataContext as MainViewModel)?.Navigation;

        [RelayCommand]
        private async Task LoginAsync()
        {
            ErrorMessage = "";
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Логин и пароль обязательны.";
                return;
            }

            var profile = await _profileRepo.GetByLoginAsync(Login);
            if (profile == null || !profile.CheckPassword(Password))
            {
                ErrorMessage = "Неверный логин или пароль.";
                return;
            }

            var mainVm = App.Current.MainWindow.DataContext as MainViewModel;
            mainVm?.NavigateToTasks(profile.Id);
        }

        [RelayCommand]
        private void GoToRegister()
        {
            Navigation?.NavigateTo<RegisterViewModel>();
        }
    }
}
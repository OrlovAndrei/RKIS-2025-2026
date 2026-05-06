using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;

namespace TodoApp.Desktop.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ProfileRepository _profileRepo = new();

        [ObservableProperty]
        private string _login = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                ErrorMessage = string.Empty;

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

                if (Application.Current.MainWindow.DataContext is MainViewModel mainVm)
                {
                    mainVm.NavigateToTasks(profile.Id);
                }
                else
                {
                    ErrorMessage = "Ошибка: MainViewModel не найден.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "Ошибка входа",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void GoToRegister()
        {
            if (Application.Current.MainWindow.DataContext is MainViewModel mainVm)
            {
                mainVm.NavigateToRegisterView();
            }
        }
    }
}
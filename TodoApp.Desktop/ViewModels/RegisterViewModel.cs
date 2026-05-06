using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly ProfileRepository _profileRepo = new();

        [ObservableProperty]
        private string _login = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _firstName = string.Empty;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private int _birthYear = DateTime.Now.Year - 18;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [RelayCommand]
        private async Task RegisterAsync()
        {
            try
            {
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Login))
                {
                    ErrorMessage = "Логин обязателен.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Пароль обязателен.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    ErrorMessage = "Имя обязательно.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(LastName))
                {
                    ErrorMessage = "Фамилия обязательна.";
                    return;
                }

                if (BirthYear < 1900 || BirthYear > DateTime.Now.Year)
                {
                    ErrorMessage = $"Год рождения должен быть между 1900 и {DateTime.Now.Year}";
                    return;
                }

                var existing = await _profileRepo.GetByLoginAsync(Login);

                if (existing != null)
                {
                    ErrorMessage = "Логин уже занят.";
                    return;
                }

                var profile = new Profile(Login, Password, FirstName, LastName, BirthYear);

                await _profileRepo.AddAsync(profile);

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
                    "Ошибка регистрации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            if (Application.Current.MainWindow.DataContext is MainViewModel mainVm)
            {
                mainVm.NavigateToLoginView();
            }
        }
    }
}
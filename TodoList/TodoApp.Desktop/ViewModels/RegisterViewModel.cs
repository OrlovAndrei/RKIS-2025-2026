using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly ProfileRepository _profileRepo = new();

        [ObservableProperty]
        private string login = "";

        [ObservableProperty]
        private string password = "";

        [ObservableProperty]
        private string firstName = "";

        [ObservableProperty]
        private string lastName = "";

        [ObservableProperty]
        private int birthYear = DateTime.Now.Year - 18;

        [ObservableProperty]
        private string errorMessage = "";

        private INavigationService? Navigation =>
            (App.Current.MainWindow.DataContext as MainViewModel)?.Navigation;

        [RelayCommand]
        private async Task RegisterAsync()
        {
            ErrorMessage = "";
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = "Все поля обязательны.";
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

            var mainVm = App.Current.MainWindow.DataContext as MainViewModel;
            mainVm?.NavigateToTasks(profile.Id);
        }

        [RelayCommand]
        private void Cancel()
        {
            Navigation?.NavigateTo<LoginViewModel>();
        }
    }
}
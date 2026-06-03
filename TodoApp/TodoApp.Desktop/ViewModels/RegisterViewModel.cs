using System;
using System.Windows.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly ProfileRepository _profileRepository;
        private readonly Action<Profile> _registered;
        private readonly Action _showLogin;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _birthYear = string.Empty;
        private string _message = string.Empty;

        public RegisterViewModel(ProfileRepository profileRepository, Action<Profile> registered, Action showLogin)
        {
            _profileRepository = profileRepository;
            _registered = registered;
            _showLogin = showLogin;
            RegisterCommand = new RelayCommand(_ => Register());
            ShowLoginCommand = new RelayCommand(_ => _showLogin());
        }

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string BirthYear
        {
            get => _birthYear;
            set => SetProperty(ref _birthYear, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand ShowLoginCommand { get; }

        private void Register()
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                Message = "Заполните все поля.";
                return;
            }

            if (!int.TryParse(BirthYear, out int year) || year < 1900 || year > DateTime.Now.Year)
            {
                Message = $"Год рождения должен быть числом 1900-{DateTime.Now.Year}.";
                return;
            }

            if (_profileRepository.LoginExists(Login.Trim()))
            {
                Message = "Такой логин уже существует.";
                return;
            }

            var profile = new Profile(Login.Trim(), Password, FirstName.Trim(), LastName.Trim(), year);
            _profileRepository.Add(profile);
            Message = string.Empty;
            _registered(profile);
        }
    }
}

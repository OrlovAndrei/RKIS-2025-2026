using System;
using System.Windows.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ProfileRepository _profileRepository;
        private readonly Action<Profile> _login;
        private readonly Action _showRegister;
        private string _loginText = string.Empty;
        private string _password = string.Empty;
        private string _message = string.Empty;

        public LoginViewModel(ProfileRepository profileRepository, Action<Profile> login, Action showRegister)
        {
            _profileRepository = profileRepository;
            _login = login;
            _showRegister = showRegister;
            LoginCommand = new RelayCommand(_ => Login());
            ShowRegisterCommand = new RelayCommand(_ => _showRegister());
        }

        public string LoginText
        {
            get => _loginText;
            set => SetProperty(ref _loginText, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand ShowRegisterCommand { get; }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(LoginText) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "Введите логин и пароль.";
                return;
            }

            var profile = _profileRepository.GetByLogin(LoginText.Trim());
            if (profile == null || profile.Password != Password)
            {
                Message = "Неверный логин или пароль.";
                return;
            }

            Message = string.Empty;
            _login(profile);
        }
    }
}

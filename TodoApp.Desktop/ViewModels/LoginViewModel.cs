using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly ProfileRepository _repo = new();
    private readonly MainViewModel _main;

    private string _login = string.Empty;
    private string _password = string.Empty;
    private string _error = string.Empty;

    public string Login
    {
        get => _login;
        set { _login = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public string Error
    {
        get => _error;
        set { _error = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; }
    public ICommand GoToRegisterCommand { get; }

    public LoginViewModel(MainViewModel main)
    {
        _main = main;
        LoginCommand = new RelayCommand(ExecuteLogin);
        GoToRegisterCommand = new RelayCommand(ExecuteGoToRegister);
    }

    private void ExecuteLogin()
    {
        if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Заполните все поля";
            return;
        }

        Profile? profile = _repo.GetByLogin(Login);
        if (profile != null && profile.CheckPassword(Password))
        {
            Error = string.Empty;
            _main.OnLoginSuccess(profile);
        }
        else
        {
            Error = "Неверный логин или пароль";
        }
    }

    private void ExecuteGoToRegister()
    {
        _main.CurrentViewModel = new RegisterViewModel(_main);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
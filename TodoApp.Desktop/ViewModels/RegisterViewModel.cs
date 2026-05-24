using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class RegisterViewModel : INotifyPropertyChanged
{
    private readonly ProfileRepository _repo = new();
    private readonly MainViewModel _main;

    private string _login = string.Empty;
    private string _password = string.Empty;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private int _birthYear = 2000;
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

    public string FirstName
    {
        get => _firstName;
        set { _firstName = value; OnPropertyChanged(); }
    }

    public string LastName
    {
        get => _lastName;
        set { _lastName = value; OnPropertyChanged(); }
    }

    public int BirthYear
    {
        get => _birthYear;
        set { _birthYear = value; OnPropertyChanged(); }
    }

    public string Error
    {
        get => _error;
        set { _error = value; OnPropertyChanged(); }
    }

    public ICommand RegisterCommand { get; }
    public ICommand GoToLoginCommand { get; }

    public RegisterViewModel(MainViewModel main)
    {
        _main = main;
        RegisterCommand = new RelayCommand(ExecuteRegister);
        GoToLoginCommand = new RelayCommand(ExecuteGoToLogin);
    }

    private void ExecuteRegister()
    {
        if (string.IsNullOrWhiteSpace(Login))
        {
            Error = "Логин не может быть пустым";
            return;
        }
        if (_repo.LoginExists(Login))
        {
            Error = "Логин уже занят";
            return;
        }
        if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            Error = "Заполните все поля";
            return;
        }
        if (BirthYear < 1900 || BirthYear > DateTime.Now.Year)
        {
            Error = "Некорректный год рождения";
            return;
        }

        var profile = new Profile(Login, Password, FirstName, LastName, BirthYear);
        _repo.Add(profile);
        _main.OnLoginSuccess(profile);
    }

    private void ExecuteGoToLogin()
    {
        _main.CurrentViewModel = new LoginViewModel(_main);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
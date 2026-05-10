using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private INavigationService? _navigation;

        [ObservableProperty]
        private ObservableObject? _currentViewModel;

        public INavigationService? Navigation => _navigation;

        public void Initialize(INavigationService navigation)
        {
            _navigation = navigation;
        }

        [RelayCommand]
        private void NavigateToLogin()
        {
            NavigateToLoginView();
        }

        [RelayCommand]
        private void NavigateToRegister()
        {
            NavigateToRegisterView();
        }

        public void NavigateToLoginView()
        {
            CurrentViewModel = new LoginViewModel();
        }

        public void NavigateToRegisterView()
        {
            CurrentViewModel = new RegisterViewModel();
        }

        public void NavigateToTasks(Guid profileId)
        {
            try
            {
                CurrentViewModel = new TodoListViewModel(profileId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "Ошибка перехода к задачам",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
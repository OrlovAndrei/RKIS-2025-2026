using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private INavigationService? _navigation;

        [ObservableProperty]
        private ObservableObject? currentViewModel;

        public INavigationService? Navigation => _navigation;

        public void Initialize(INavigationService navigation)
        {
            _navigation = navigation;
        }

        [RelayCommand]
        private void NavigateToLogin() => _navigation?.NavigateTo<LoginViewModel>();

        [RelayCommand]
        private void NavigateToRegister() => _navigation?.NavigateTo<RegisterViewModel>();

        public void NavigateToTasks(Guid profileId)
        {
            _navigation?.NavigateTo(new TodoListViewModel(profileId));
        }
    }
}
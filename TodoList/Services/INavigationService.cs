using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.Desktop.Services
{
    public interface INavigationService
    {
        ObservableObject? CurrentViewModel { get; set; }
        void NavigateTo(ObservableObject viewModel);
        void GoBack();
        bool CanGoBack { get; }
        event Action? CurrentViewModelChanged;
    }
}
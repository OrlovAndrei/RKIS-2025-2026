using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.Desktop.Services
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>() where TViewModel : ObservableObject, new();
        void NavigateTo(ObservableObject viewModel);
    }
}
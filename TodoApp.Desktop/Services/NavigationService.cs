using CommunityToolkit.Mvvm.ComponentModel;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Services
{
    public class NavigationService : INavigationService
    {
        private readonly MainViewModel _mainVm;

        public NavigationService(MainViewModel mainVm)
        {
            _mainVm = mainVm;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ObservableObject, new()
        {
            _mainVm.CurrentViewModel = new TViewModel();
        }

        public void NavigateTo(ObservableObject viewModel)
        {
            _mainVm.CurrentViewModel = viewModel;
        }
    }
}
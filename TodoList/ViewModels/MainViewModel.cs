using CommunityToolkit.Mvvm.Input;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ObservableObject? CurrentViewModel => _navigationService.CurrentViewModel;
        public bool CanGoBack => _navigationService.CanGoBack;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.CurrentViewModelChanged += () =>
            {
                OnPropertyChanged(nameof(CurrentViewModel));
                OnPropertyChanged(nameof(CanGoBack));
            };
        }

        [RelayCommand]
        private void GoBack()
        {
            if (_navigationService.CanGoBack)
                _navigationService.GoBack();
        }
    }
}
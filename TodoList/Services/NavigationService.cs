using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.Desktop.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Stack<ObservableObject> _history = new();
        private ObservableObject? _currentViewModel;

        public ObservableObject? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != null && value != null && _currentViewModel.GetType() != value.GetType())
                    _history.Push(_currentViewModel);
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }

        public event Action? CurrentViewModelChanged;

        public void NavigateTo(ObservableObject viewModel)
        {
            CurrentViewModel = viewModel;
        }

        public void GoBack()
        {
            if (_history.Count > 0)
                CurrentViewModel = _history.Pop();
        }

        public bool CanGoBack => _history.Count > 0;
    }
}
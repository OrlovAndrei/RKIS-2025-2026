using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.Desktop.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        private bool _isBusy;
        private string? _errorMessage;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
    }
}
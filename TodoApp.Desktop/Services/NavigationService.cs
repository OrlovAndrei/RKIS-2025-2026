using System;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Services
{
    public class NavigationService : INavigationService
    {
        private readonly MainViewModel _mainViewModel;

        public NavigationService(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
        }

        public void NavigateTo(object viewModel)
        {
            _mainViewModel.CurrentViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }
}
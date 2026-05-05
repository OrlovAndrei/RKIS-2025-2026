using System.Windows;
using TodoApp.Desktop.Views;

namespace TodoApp.Desktop.Services
{
    public class DialogService : IDialogService
    {
        public bool? ShowDialog(object viewModel)
        {
            var window = new AddEditTaskWindow { DataContext = viewModel };
            return window.ShowDialog();
        }
    }
}
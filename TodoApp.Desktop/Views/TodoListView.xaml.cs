using System.Windows.Controls;
using System.Windows.Input;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Views
{
    public partial class TodoListView : UserControl
    {
        public TodoListView()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is TodoListViewModel vm && vm.EditTaskCommand.CanExecute(null))
            {
                vm.EditTaskCommand.Execute(null);
            }
        }
    }
}
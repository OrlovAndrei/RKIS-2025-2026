using System.Windows.Controls;

namespace TodoApp.Desktop.Views
{
    public partial class TodoListView : UserControl
    {
        public TodoListView()
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                if (DataContext is ViewModels.TodoListViewModel vm)
                    await vm.LoadTodosCommand.ExecuteAsync(null);
            };
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Views
{
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm && PasswordBox != null)
            {
                vm.Password = PasswordBox.Password;
            }
        }
    }
}
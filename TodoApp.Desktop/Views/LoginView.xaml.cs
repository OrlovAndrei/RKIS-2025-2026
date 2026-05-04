using System.Windows;
using System.Windows.Controls;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm && PasswordBox != null)
            {
                vm.Password = PasswordBox.Password;
            }
        }
    }
}
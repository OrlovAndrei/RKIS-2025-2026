using System.Windows.Controls;

namespace TodoApp.Desktop.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                if (DataContext is ViewModels.LoginViewModel vm)
                    PasswordBox.PasswordChanged += (_, _) => vm.Password = PasswordBox.Password;
            };
        }
    }
}
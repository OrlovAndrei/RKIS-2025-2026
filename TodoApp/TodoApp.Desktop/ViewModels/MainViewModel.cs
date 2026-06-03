using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ProfileRepository _profileRepository = new();
        private readonly TodoRepository _todoRepository = new();
        private ViewModelBase _currentViewModel;

        public MainViewModel()
        {
            using (var context = new AppDbContext())
            {
                context.Database.Migrate();
            }

            _currentViewModel = CreateLoginViewModel();
        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        private LoginViewModel CreateLoginViewModel()
        {
            return new LoginViewModel(_profileRepository, Login, ShowRegister);
        }

        private RegisterViewModel CreateRegisterViewModel()
        {
            return new RegisterViewModel(_profileRepository, Login, ShowLogin);
        }

        private void Login(Profile profile)
        {
            CurrentViewModel = new TodoListViewModel(_todoRepository, profile);
        }

        private void ShowLogin()
        {
            CurrentViewModel = CreateLoginViewModel();
        }

        private void ShowRegister()
        {
            CurrentViewModel = CreateRegisterViewModel();
        }
    }
}

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;
using TodoApp.Data;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels
{
    public partial class TodoListViewModel : ViewModelBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public ObservableCollection<TodoItem> Todos { get; } = new();

        [ObservableProperty]
        private TodoItem? _selectedTodo;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private TodoStatus? _statusFilter;

        [ObservableProperty]
        private DateTime? _dueDateFilter;

        public IEnumerable<TodoStatus> StatusValues => Enum.GetValues(typeof(TodoStatus)).Cast<TodoStatus>();

        public TodoListViewModel(ITodoRepository todoRepository, IProfileRepository profileRepository,
                                 INavigationService navigationService, IDialogService dialogService)
        {
            _todoRepository = todoRepository;
            _profileRepository = profileRepository;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        [RelayCommand]
        private async Task LoadTodosAsync()
        {
            await ExecuteBusyAsync(async () =>
            {
                var profile = await _profileRepository.GetCurrentProfileAsync();
                if (profile == null)
                {
                    var loginVm = new LoginViewModel(_todoRepository, _profileRepository, _navigationService, _dialogService);
                    _navigationService.NavigateTo(loginVm);
                    return;
                }

                var items = await _todoRepository.GetAllAsync(profile.Id);
                UpdateCollection(items);
            });
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            await ExecuteBusyAsync(async () =>
            {
                var profile = await _profileRepository.GetCurrentProfileAsync();
                if (profile == null) return;

                var items = await _todoRepository.SearchAsync(profile.Id, SearchText, StatusFilter, DueDateFilter);
                UpdateCollection(items);
            });
        }

        [RelayCommand]
        private void ClearFilters()
        {
            SearchText = string.Empty;
            StatusFilter = null;
            DueDateFilter = null;
            _ = SearchAsync();
        }

        [RelayCommand]
        private void AddTodo()
        {
            var vm = new AddEditTodoViewModel(_todoRepository, _profileRepository, _navigationService, _dialogService);
            vm.TodoSaved += OnTodoSaved;
            _navigationService.NavigateTo(vm);
        }

        [RelayCommand]
        private void EditTodo()
        {
            if (SelectedTodo == null) return;
            var vm = new AddEditTodoViewModel(_todoRepository, _profileRepository, _navigationService, _dialogService, SelectedTodo);
            vm.TodoSaved += OnTodoSaved;
            _navigationService.NavigateTo(vm);
        }

        [RelayCommand]
        private async Task DeleteTodoAsync()
        {
            if (SelectedTodo == null) return;
            var confirmed = await _dialogService.ShowConfirmationAsync($"Удалить задачу \"{SelectedTodo.Text}\"?");
            if (!confirmed) return;

            await ExecuteBusyAsync(async () =>
            {
                await _todoRepository.DeleteAsync(SelectedTodo.Id);
                await LoadTodosAsync();
            });
        }

        [RelayCommand]
        private async Task ChangeStatusAsync(TodoStatus newStatus)
        {
            if (SelectedTodo == null) return;
            await ExecuteBusyAsync(async () =>
            {
                await _todoRepository.SetStatusAsync(SelectedTodo.Id, newStatus);
                await LoadTodosAsync();
            });
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            await _profileRepository.LogoutAsync();
            var loginVm = new LoginViewModel(_todoRepository, _profileRepository, _navigationService, _dialogService);
            _navigationService.NavigateTo(loginVm);
        }

        private void UpdateCollection(List<TodoItem> items)
        {
            Todos.Clear();
            foreach (var item in items)
                Todos.Add(item);
        }

        private async void OnTodoSaved()
        {
            await LoadTodosAsync();
            _navigationService.GoBack();
        }

        private async Task ExecuteBusyAsync(Func<Task> action)
        {
            IsBusy = true;
            ErrorMessage = null;
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                await _dialogService.ShowErrorAsync(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

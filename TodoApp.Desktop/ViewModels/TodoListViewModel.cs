using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public partial class TodoListViewModel : ObservableObject
    {
        private readonly Guid _profileId;
        private readonly TodoRepository _todoRepo = new();
        private readonly ProfileRepository _profileRepo = new();
        private readonly IDialogService _dialogService = new DialogService();

        public ObservableCollection<TodoItem> Tasks { get; } = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTaskSelected))]
        private TodoItem? _selectedTask;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private TodoStatus? _filterStatus;

        [ObservableProperty]
        private string _sortBy = "date";

        [ObservableProperty]
        private bool _sortDescending;

        [ObservableProperty]
        private Profile? _currentProfile;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        public bool IsTaskSelected => SelectedTask != null;

        public ICommand ChangeStatusCommand => new RelayCommand<TodoItem?>(ChangeStatus);

        private INavigationService? Navigation =>
            (Application.Current.MainWindow.DataContext as MainViewModel)?.Navigation;

        public TodoListViewModel(Guid profileId)
        {
            _profileId = profileId;
            Task.Run(LoadAsync);
        }

        private async Task LoadAsync()
        {
            try
            {
                CurrentProfile = await _profileRepo.GetByIdAsync(_profileId);
                if (CurrentProfile == null)
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        StatusMessage = "Профиль не найден в базе данных.";
                    });
                    return;
                }

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    StatusMessage = $"Загружены задачи пользователя: {CurrentProfile.FullName}";
                    ApplyFilters();
                });
            }
            catch (Exception ex)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    StatusMessage = $"Ошибка при загрузке задач: {ex.Message}";
                });
            }
        }

        partial void OnSearchTextChanged(string value) => ApplyFilters();
        partial void OnFilterStatusChanged(TodoStatus? value) => ApplyFilters();
        partial void OnSortByChanged(string value) => ApplyFilters();
        partial void OnSortDescendingChanged(bool value) => ApplyFilters();

        private async void ApplyFilters()
        {
            var allItems = await _todoRepo.GetAllForProfileAsync(_profileId);
            IEnumerable<TodoItem> query = allItems;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(t => t.Text.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (FilterStatus.HasValue)
            {
                query = query.Where(t => t.Status == FilterStatus.Value);
            }

            query = SortBy switch
            {
                "text" => SortDescending
                    ? query.OrderByDescending(t => t.Text)
                    : query.OrderBy(t => t.Text),
                _ => SortDescending
                    ? query.OrderByDescending(t => t.LastUpdate)
                    : query.OrderBy(t => t.LastUpdate)
            };

            var filtered = query.ToList();
            
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Tasks.Clear();
                foreach (var item in filtered) Tasks.Add(item);
                StatusMessage = $"Найдено задач: {Tasks.Count}";
            });
        }

        [RelayCommand]
        private async Task AddTaskAsync()
        {
            var vm = new TaskEditViewModel(_profileId, isNew: true);
            if (_dialogService.ShowDialog(vm) == true)
            {
                await LoadAsync();
                ApplyFilters();
            }
        }

        [RelayCommand(CanExecute = nameof(IsTaskSelected))]
        private async Task EditTaskAsync()
        {
            if (SelectedTask == null) return;
            var vm = new TaskEditViewModel(_profileId, isNew: false, itemId: SelectedTask.Id);
            if (_dialogService.ShowDialog(vm) == true)
            {
                await LoadAsync();
                ApplyFilters();
            }
        }

        [RelayCommand(CanExecute = nameof(IsTaskSelected))]
        private async Task DeleteTaskAsync()
        {
            if (SelectedTask == null) return;
            
            var result = MessageBox.Show($"Удалить задачу \"{SelectedTask.GetShortInfo()}\"?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                await _todoRepo.DeleteAsync(SelectedTask.Id);
                await LoadAsync();
                ApplyFilters();
                StatusMessage = "Задача удалена";
            }
        }

        private async void ChangeStatus(TodoItem? item)
        {
            if (item == null) return;
            
            var newStatus = item.Status switch
            {
                TodoStatus.NotStarted => TodoStatus.InProgress,
                TodoStatus.InProgress => TodoStatus.Completed,
                TodoStatus.Completed => TodoStatus.Postponed,
                TodoStatus.Postponed => TodoStatus.Failed,
                TodoStatus.Failed => TodoStatus.NotStarted,
                _ => TodoStatus.NotStarted
            };
            
            await _todoRepo.SetStatusAsync(item.Id, newStatus);
            item.Status = newStatus;
            
            var index = Tasks.IndexOf(item);
            if (index >= 0)
            {
                Tasks.RemoveAt(index);
                Tasks.Insert(index, item);
            }
            
            StatusMessage = $"Статус задачи изменен на {newStatus}";
        }

        [RelayCommand]
        private void Logout()
        {
            Navigation?.NavigateTo<LoginViewModel>();
        }
    }
}
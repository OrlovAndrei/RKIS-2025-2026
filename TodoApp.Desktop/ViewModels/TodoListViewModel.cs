using System;
using System.Collections.Generic;
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

        public IEnumerable<TodoStatus> TodoStatusValues =>
            Enum.GetValues<TodoStatus>();

        [ObservableProperty]
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

        public ICommand ChangeStatusCommand { get; }

        private INavigationService? Navigation =>
            (Application.Current.MainWindow.DataContext as MainViewModel)?.Navigation;

        public TodoListViewModel(Guid profileId)
        {
            _profileId = profileId;
            ChangeStatusCommand = new RelayCommand<TodoItem?>(ChangeStatus);

            _ = LoadAsync();
        }

        partial void OnSelectedTaskChanged(TodoItem? value)
        {
            OnPropertyChanged(nameof(IsTaskSelected));

            EditTaskCommand.NotifyCanExecuteChanged();
            DeleteTaskCommand.NotifyCanExecuteChanged();
        }

        private async Task LoadAsync()
        {
            try
            {
                CurrentProfile = await _profileRepo.GetByIdAsync(_profileId);

                if (CurrentProfile == null)
                {
                    StatusMessage = "Профиль не найден в базе данных.";
                    return;
                }

                await ApplyFiltersAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при загрузке задач: {ex.Message}";
            }
        }

        partial void OnSearchTextChanged(string value) => _ = ApplyFiltersAsync();
        partial void OnFilterStatusChanged(TodoStatus? value) => _ = ApplyFiltersAsync();
        partial void OnSortByChanged(string value) => _ = ApplyFiltersAsync();
        partial void OnSortDescendingChanged(bool value) => _ = ApplyFiltersAsync();

        private async Task ApplyFiltersAsync()
        {
            try
            {
                var selectedId = SelectedTask?.Id;

                var allItems = await _todoRepo.GetAllForProfileAsync(_profileId);
                IEnumerable<TodoItem> query = allItems;

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    query = query.Where(t =>
                        t.Text.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
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

                Tasks.Clear();

                foreach (var item in filtered)
                {
                    Tasks.Add(item);
                }

                SelectedTask = selectedId.HasValue
                    ? Tasks.FirstOrDefault(t => t.Id == selectedId.Value)
                    : null;

                StatusMessage = $"Найдено задач: {Tasks.Count}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при фильтрации задач: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task AddTaskAsync()
        {
            var vm = new TaskEditViewModel(_profileId, isNew: true);

            _dialogService.ShowDialog(vm);

            await LoadAsync();
        }

        private bool CanModifySelectedTask()
        {
            return SelectedTask != null;
        }

        [RelayCommand(CanExecute = nameof(CanModifySelectedTask))]
        private async Task EditTaskAsync()
        {
            if (SelectedTask == null)
            {
                return;
            }

            var vm = new TaskEditViewModel(_profileId, isNew: false, itemId: SelectedTask.Id);

            _dialogService.ShowDialog(vm);

            await LoadAsync();
        }

        [RelayCommand(CanExecute = nameof(CanModifySelectedTask))]
        private async Task DeleteTaskAsync()
        {
            if (SelectedTask == null)
            {
                return;
            }

            var result = MessageBox.Show(
                $"Удалить задачу \"{SelectedTask.GetShortInfo()}\"?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            await _todoRepo.DeleteAsync(SelectedTask.Id);

            SelectedTask = null;
            await LoadAsync();

            StatusMessage = "Задача удалена";
        }

        private async void ChangeStatus(TodoItem? item)
        {
            if (item == null)
            {
                return;
            }

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
            await LoadAsync();

            StatusMessage = $"Статус задачи изменен на {newStatus}";
        }

        [RelayCommand]
        private void Logout()
        {
            Navigation?.NavigateTo<LoginViewModel>();
        }
    }
}
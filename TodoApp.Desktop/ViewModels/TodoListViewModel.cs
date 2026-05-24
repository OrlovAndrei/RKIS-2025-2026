using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public partial class TodoListViewModel : ObservableObject
    {
        private readonly TodoRepository _todoRepo = new();
        private readonly MainViewModel _main;
        private readonly Guid _profileId;

        public ObservableCollection<TodoItem> Tasks { get; } = new();
        
        public ObservableCollection<TodoStatus> AvailableStatuses { get; } = new(Enum.GetValues<TodoStatus>());

        [ObservableProperty] private string _searchText = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditTaskCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteTaskCommand))]
        [NotifyCanExecuteChangedFor(nameof(ChangeStatusCommand))]
        private TodoItem? _selectedTask;

        [ObservableProperty] private TodoStatus? _filterStatus;

        public TodoListViewModel(Guid profileId, MainViewModel main)
        {
            _profileId = profileId;
            _main = main;
            LoadTasks();
        }

        private void LoadTasks()
        {
            Tasks.Clear();
            var all = _todoRepo.GetAllByProfileId(_profileId);
            foreach (var t in all) Tasks.Add(t);
        }

        partial void OnSearchTextChanged(string value) => ApplyFilters();
        partial void OnFilterStatusChanged(TodoStatus? value) => ApplyFilters();

        private void ApplyFilters()
        {
            var all = _todoRepo.GetAllByProfileId(_profileId);
            var query = all.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(t => t.Text.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            if (FilterStatus.HasValue)
                query = query.Where(t => t.Status == FilterStatus.Value);

            Tasks.Clear();
            foreach (var t in query) Tasks.Add(t);
        }

        [RelayCommand] private void Refresh() => LoadTasks();
        [RelayCommand] private void AddTask() => _main.NavigateToAddTask();

        [RelayCommand(CanExecute = nameof(CanModifyTask))]
        private void EditTask() => _main.NavigateToEditTask(SelectedTask!);

        [RelayCommand(CanExecute = nameof(CanModifyTask))]
        private void DeleteTask()
        {
            if (SelectedTask == null) return;
            _todoRepo.Delete(SelectedTask.Id);
            LoadTasks();
        }

        [RelayCommand(CanExecute = nameof(CanModifyTask))]
        private void ChangeStatus()
        {
            if (SelectedTask == null) return;

            var statuses = Enum.GetValues<TodoStatus>();
            int currentIndex = Array.IndexOf(statuses, SelectedTask.Status);
            int nextIndex = (currentIndex + 1) % statuses.Length;

            SelectedTask.SetStatus(statuses[nextIndex]);
            _todoRepo.Update(SelectedTask);
            LoadTasks();
        }

        private bool CanModifyTask => SelectedTask != null;
    }
}
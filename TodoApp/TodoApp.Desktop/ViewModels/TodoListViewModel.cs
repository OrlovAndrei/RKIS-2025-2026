using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public class TodoListViewModel : ViewModelBase
    {
        private readonly TodoRepository _todoRepository;
        private readonly Profile _profile;
        private readonly TaskFormViewModel _addForm = new();
        private readonly TaskFormViewModel _editForm = new();
        private TodoItem? _selectedTask;
        private string _searchText = string.Empty;
        private TodoStatus? _statusFilter;
        private string _message = string.Empty;

        public TodoListViewModel(TodoRepository todoRepository, Profile profile)
        {
            _todoRepository = todoRepository;
            _profile = profile;
            Tasks = new ObservableCollection<TodoItem>(_todoRepository.GetAll(profile.Id));
            TasksView = CollectionViewSource.GetDefaultView(Tasks);
            TasksView.Filter = FilterTask;

            AddTaskCommand = new RelayCommand(_ => AddTask());
            EditTaskCommand = new RelayCommand(_ => EditTask(), _ => SelectedTask != null);
            DeleteTaskCommand = new RelayCommand(_ => DeleteTask(), _ => SelectedTask != null);
            RefreshCommand = new RelayCommand(_ => Refresh());
            ClearFilterCommand = new RelayCommand(_ => ClearFilter());
        }

        public string Title => $"Задачи: {_profile.DisplayName}";

        public ObservableCollection<TodoItem> Tasks { get; }

        public ICollectionView TasksView { get; }

        public TaskFormViewModel AddForm => _addForm;

        public TaskFormViewModel EditForm => _editForm;

        public Array Statuses => Enum.GetValues(typeof(TodoStatus));

        public TodoItem? SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (SetProperty(ref _selectedTask, value))
                {
                    EditForm.Load(value);
                    RaiseCommandStates();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    TasksView.Refresh();
                }
            }
        }

        public TodoStatus? StatusFilter
        {
            get => _statusFilter;
            set
            {
                if (SetProperty(ref _statusFilter, value))
                {
                    TasksView.Refresh();
                }
            }
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand AddTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ClearFilterCommand { get; }

        private void AddTask()
        {
            if (string.IsNullOrWhiteSpace(AddForm.Text))
            {
                Message = "Введите текст задачи.";
                return;
            }

            var task = new TodoItem(AddForm.Text.Trim())
            {
                ProfileId = _profile.Id,
                Status = AddForm.Status
            };

            _todoRepository.Add(task);
            AddForm.Load(null);
            Message = "Задача добавлена.";
            Refresh();
        }

        private void EditTask()
        {
            if (SelectedTask == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(EditForm.Text))
            {
                Message = "Введите текст задачи.";
                return;
            }

            SelectedTask.Text = EditForm.Text.Trim();
            SelectedTask.Status = EditForm.Status;
            SelectedTask.LastUpdate = DateTime.Now;
            _todoRepository.Update(SelectedTask);
            Message = "Задача обновлена.";
            Refresh();
        }

        private void DeleteTask()
        {
            if (SelectedTask == null)
            {
                return;
            }

            _todoRepository.Delete(SelectedTask.Id);
            Message = "Задача удалена.";
            Refresh();
        }

        private void Refresh()
        {
            Tasks.Clear();
            foreach (var task in _todoRepository.GetAll(_profile.Id))
            {
                Tasks.Add(task);
            }

            SelectedTask = Tasks.FirstOrDefault();
            TasksView.Refresh();
        }

        private void ClearFilter()
        {
            SearchText = string.Empty;
            StatusFilter = null;
        }

        private bool FilterTask(object item)
        {
            if (item is not TodoItem task)
            {
                return false;
            }

            bool textMatches = string.IsNullOrWhiteSpace(SearchText) ||
                task.Text.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            bool statusMatches = !StatusFilter.HasValue || task.Status == StatusFilter.Value;

            return textMatches && statusMatches;
        }

        private void RaiseCommandStates()
        {
            (EditTaskCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteTaskCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}

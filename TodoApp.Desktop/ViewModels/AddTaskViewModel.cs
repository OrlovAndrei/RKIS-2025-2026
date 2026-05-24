using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public partial class AddTaskViewModel : ObservableObject
    {
        private readonly TodoRepository _todoRepo = new();
        private readonly MainViewModel _main;
        private readonly Guid _profileId;

        [ObservableProperty] private string _taskText = string.Empty;
        [ObservableProperty] private string _validationMessage = string.Empty;

        public AddTaskViewModel(Guid profileId, MainViewModel main)
        {
            _profileId = profileId;
            _main = main;
        }

        [RelayCommand]
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(TaskText))
            {
                ValidationMessage = "Текст задачи не может быть пустым.";
                return;
            }

            try
            {
                var newItem = new TodoItem(TaskText) { ProfileId = _profileId };
                
                _todoRepo.Add(newItem);
                ValidationMessage = string.Empty;

                _main.CurrentViewModel = new TodoListViewModel(_profileId, _main);
            }
            catch (Exception ex)
            {
                ValidationMessage = $"Ошибка сохранения: {ex.Message}";
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            _main.CurrentViewModel = new TodoListViewModel(_profileId, _main);
        }
    }
}
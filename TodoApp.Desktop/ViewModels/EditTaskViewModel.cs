using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public partial class EditTaskViewModel : ObservableObject
    {
        private readonly TodoRepository _todoRepo = new();
        private readonly MainViewModel _main;
        private readonly TodoItem _originalItem;

        [ObservableProperty] private string _taskText = string.Empty;
        [ObservableProperty] private string _validationMessage = string.Empty;

        public EditTaskViewModel(TodoItem item, MainViewModel main)
        {
            _originalItem = item;
            _main = main;
            
            TaskText = item.Text;
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
                _originalItem.UpdateText(TaskText);
                
                _todoRepo.Update(_originalItem);
                ValidationMessage = string.Empty;

                _main.CurrentViewModel = new TodoListViewModel(_originalItem.ProfileId, _main);
            }
            catch (Exception ex)
            {
                ValidationMessage = $"Ошибка обновления: {ex.Message}";
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            _main.CurrentViewModel = new TodoListViewModel(_originalItem.ProfileId, _main);
        }
    }
}
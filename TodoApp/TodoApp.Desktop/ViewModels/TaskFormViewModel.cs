using System;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public class TaskFormViewModel : ViewModelBase
    {
        private string _text = string.Empty;
        private TodoStatus _status = TodoStatus.NotStarted;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public TodoStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public Array Statuses => Enum.GetValues(typeof(TodoStatus));

        public void Load(TodoItem? item)
        {
            Text = item?.Text ?? string.Empty;
            Status = item?.Status ?? TodoStatus.NotStarted;
        }
    }
}

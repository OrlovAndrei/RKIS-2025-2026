using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels
{
    public partial class TaskEditViewModel : ObservableObject
    {
        private readonly Guid _profileId;
        private readonly int? _itemId;
        private readonly TodoRepository _todoRepo = new();

        [ObservableProperty]
        private string text = "";

        [ObservableProperty]
        private TodoStatus status = TodoStatus.NotStarted;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public TaskEditViewModel(Guid profileId, bool isNew, int? itemId = null)
        {
            _profileId = profileId;
            _itemId = itemId;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            if (!isNew && itemId.HasValue)
            {
                LoadExisting(itemId.Value);
            }
        }

        private async void LoadExisting(int id)
        {
            var item = await _todoRepo.GetByIdAsync(id);
            if (item != null)
            {
                Text = item.Text;
                Status = item.Status;
            }
        }

        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(Text)) return;

            TodoItem item;
            if (_itemId.HasValue)
            {
                item = await _todoRepo.GetByIdAsync(_itemId.Value);
                if (item == null) return;
                item.Text = Text;
                item.Status = Status;
                item.LastUpdate = DateTime.Now;
                await _todoRepo.UpdateAsync(item);
            }
            else
            {
                item = new TodoItem(Text, Status, DateTime.Now) { ProfileId = _profileId };
                await _todoRepo.AddAsync(item);
            }

            if (GetWindow() is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        private void Cancel()
        {
            GetWindow()?.Close();
        }

        private Window? GetWindow() =>
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this);
    }
}
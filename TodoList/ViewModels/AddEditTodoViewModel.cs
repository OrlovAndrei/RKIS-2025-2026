using CommunityToolkit.Mvvm.Input;
using TodoApp.Models;
using TodoApp.Data;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels
{
    public partial class AddEditTodoViewModel : ViewModelBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly TodoItem? _editingItem;

        [ObservableProperty]
        private string _text = string.Empty;

        public string Title => _editingItem == null ? "Добавление задачи" : "Редактирование задачи";

        public event Action? TodoSaved;

        public AddEditTodoViewModel(ITodoRepository todoRepository, IProfileRepository profileRepository,
                                    INavigationService navigationService, IDialogService dialogService, TodoItem? item = null)
        {
            _todoRepository = todoRepository;
            _profileRepository = profileRepository;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _editingItem = item;

            if (item != null)
                Text = item.Text;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                await _dialogService.ShowErrorAsync("Введите текст задачи");
                return;
            }

            IsBusy = true;

            try
            {
                var profile = await _profileRepository.GetCurrentProfileAsync();
                if (profile == null)
                {
                    await _dialogService.ShowErrorAsync("Пользователь не авторизован");
                    return;
                }

                if (_editingItem == null)
                {
                    var newItem = new TodoItem { Text = Text, ProfileId = profile.Id };
                    await _todoRepository.AddAsync(newItem);
                }
                else
                {
                    _editingItem.UpdateText(Text);
                    await _todoRepository.UpdateAsync(_editingItem);
                }

                TodoSaved?.Invoke();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync($"Ошибка сохранения: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            _navigationService.GoBack();
        }
    }
}
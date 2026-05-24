namespace TodoApp.Desktop.Services
{
    public interface IDialogService
    {
        Task<bool> ShowConfirmationAsync(string message, string title = "Подтверждение");
        Task ShowMessageAsync(string message, string title = "Сообщение");
        Task ShowErrorAsync(string message, string title = "Ошибка");
    }
}
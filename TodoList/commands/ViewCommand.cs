namespace TodoList.commands;

public class ViewCommand : ICommand
{
    public bool ShowIndex { get; set; }
    public bool ShowStatus { get; set; }
    public bool ShowDate { get; set; }
    public bool ShowAll { get; set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
        {
            Console.WriteLine("Ошибка: нет активного профиля");
            return;
        }
        
        if (ShowAll)
        {
            ShowIndex = true;
            ShowStatus = true;
            ShowDate = true;
        }

        var todoList = AppInfo.GetCurrentTodoList();
        todoList.View(ShowIndex, ShowStatus, ShowDate);
    }

    public void Unexecute()
    {
        // Команда view не требует отмены
    }
}
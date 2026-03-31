namespace TodoList;

public class DoneCommand: ICommand
{

    public string[] parts { get; set; }
    public TodoList todoList { get; set; } 
    
    public void Execute()
    {
        if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
        {
            Console.WriteLine("Ошибка: укажите номер задачи");
            return;
        }
        int index = taskNumber - 1;
        try
        {
            TodoItem item = todoList.GetItem(index);
            item.MarkDone();
            Console.WriteLine($"Задача '{item.Text}' отмечена как выполненная");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }
}
namespace TodoList;

public class UpdateCommand: ICommand
{
    public int TaskIndex { get; set; }
    public string NewText { get; set; }

    public TodoList todoList { get; set; }

    public void Execute()
    {
        todoList.Update(TaskIndex, NewText);
    }
}
namespace TodoList;

public class ReadCommand: ICommand
{
    public int TaskIndex { get; set; }
    public TodoList todoList { get; set; }

    public void Execute()
    {
        todoList.Read(TaskIndex);
    }
}
namespace TodoList.Commands;

public class DoneCommand : ICommand
{
    public int TaskIndex { get; set; }
    public TodoList todoList { get; set; }

    public void Execute()
    {
        todoList.MarkDone(TaskIndex);
    }
}
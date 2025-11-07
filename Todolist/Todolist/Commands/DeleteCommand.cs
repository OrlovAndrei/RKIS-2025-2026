namespace TodoList.Commands;

public class DeleteCommand : ICommand
{
    public int TaskIndex { get; set; }
    public TodoList todoList { get; set; }

    public void Execute()
    {
        todoList.Delete(TaskIndex);
    }
}
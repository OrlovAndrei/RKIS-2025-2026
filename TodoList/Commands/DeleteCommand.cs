namespace TodoList;

public class DeleteCommand : ICommand
{
    private readonly TodoList _todoList;
    private readonly int _index;

    public DeleteCommand(TodoList todoList, int index)
    {
        _todoList = todoList;
        _index = index;
    }

    public void Execute()
    {
        _todoList.Delete(_index);
    }
}
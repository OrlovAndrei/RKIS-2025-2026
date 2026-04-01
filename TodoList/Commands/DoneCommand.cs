namespace TodoList;

public class DoneCommand : ICommand
{
    private readonly TodoList _todoList;
    private readonly int _index;

    public DoneCommand(TodoList todoList, int index)
    {
        _todoList = todoList;
        _index = index;
    }

    public void Execute()
    {
        _todoList.MarkDone(_index);
    }
}
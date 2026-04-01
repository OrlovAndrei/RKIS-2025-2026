namespace TodoList;

public class ReadCommand : ICommand
{
    private readonly TodoList _todoList;
    private readonly int _index;

    public ReadCommand(TodoList todoList, int index)
    {
        _todoList = todoList;
        _index = index;
    }

    public void Execute()
    {
        _todoList.Read(_index);
    }
}
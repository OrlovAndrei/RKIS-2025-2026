namespace TodoList;

public class UpdateCommand : ICommand
{
    private readonly TodoList _todoList;
    private readonly int _index;
    private readonly string _newText;

    public UpdateCommand(TodoList todoList, int index, string newText)
    {
        _todoList = todoList;
        _index = index;
        _newText = newText;
    }

    public void Execute()
    {
        _todoList.Update(_index, _newText);
    }
}
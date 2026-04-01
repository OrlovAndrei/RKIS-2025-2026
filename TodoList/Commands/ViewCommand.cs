namespace TodoList;

public class ViewCommand : ICommand
{
    private readonly TodoList _todoList;
    private readonly bool _showIndex;
    private readonly bool _showStatus;
    private readonly bool _showUpdateDate;

    public ViewCommand(TodoList todoList, bool showIndex, bool showStatus, bool showUpdateDate)
    {
        _todoList = todoList;
        _showIndex = showIndex;
        _showStatus = showStatus;
        _showUpdateDate = showUpdateDate;
    }

    public void Execute()
    {
        _todoList.View(_showIndex, _showStatus, _showUpdateDate);
    }
}
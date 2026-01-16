class AddCommand : ICommand
{
    private TodoList _todoList;
    private string _text;

    public AddCommand(TodoList todoList, string text)
    {
        _todoList = todoList;
        _text = text;
    }

    public void Execute()
    {
        _todoList.Add(new TodoItem(_text));
    }
}

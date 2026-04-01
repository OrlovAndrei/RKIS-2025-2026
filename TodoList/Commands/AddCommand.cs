namespace TodoList;

public class AddCommand : ICommand
{
    private readonly TodoList _todoList;
    private readonly string _initialText;
    private readonly bool _isMultiline;

    public AddCommand(TodoList todoList, string initialText, bool isMultiline)
    {
        _todoList = todoList;
        _initialText = initialText;
        _isMultiline = isMultiline;
    }

    public void Execute()
    {
        string finalText = _initialText;
        if (_isMultiline)
        {
            Console.WriteLine("Многострочный ввод, введите !end для завершения");
            finalText = "";
            while (true)
            {
                string line = Console.ReadLine();
                if (line == "!end") break;
                finalText += line + "\n";
            }
        }

        _todoList.Add(new TodoItem(finalText));
    }
}
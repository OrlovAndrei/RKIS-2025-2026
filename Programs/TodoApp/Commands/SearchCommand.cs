class SearchCommand : ICommand
{
    void Execute()
    {
        var todos = AppInfo.GetCurrentTodoList();
        if (todos == null || todos.Count == 0)
        {                
            Console.WriteLine("Список задач пуст.");
            return;
        }

		Console.WriteLine(todos.GetTable());
    }
}
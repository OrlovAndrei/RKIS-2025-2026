namespace TodoList.commands;

public class ExitCommand : ICommand
{
    public Profile? Profile { get; set; }
    public TodoList? TodoList { get; set; }

    public void Execute()
    {
        if (Profile != null) FileManager.SaveProfile(Profile, Program.ProfileFilePath);
        if (TodoList != null) FileManager.SaveTodos(TodoList, Program.TodoFilePath);

        Console.WriteLine("Выход из программы.");
        Environment.Exit(0);
    }

    public void Unexecute()
    {
        
    }
}
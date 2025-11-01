namespace TodoList
{
    public class ExitCommand : ICommand
    {
        public void Execute()
        {
            System.Console.WriteLine("Выход.");
            System.Environment.Exit(0);
        }
    }
}
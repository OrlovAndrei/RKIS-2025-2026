namespace TodoList
{
    public class ExitCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Выход.");
            Environment.Exit(0);
        }

        public void Unexecute() { }
    }
}
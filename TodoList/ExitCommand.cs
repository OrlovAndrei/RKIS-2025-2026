namespace TodoList
{
    public class ExitCommand : BaseCommand
    {
        public override void Execute()
        {
            System.Console.WriteLine("Выход.");
            System.Environment.Exit(0); 
        }
    }
}
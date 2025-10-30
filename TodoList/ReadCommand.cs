namespace TodoList
{
    public class ReadCommand : ICommand
    {
        public int TaskNumber { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            try
            {
                var item = TodoList.GetItem(TaskNumber);
                Console.WriteLine($"Задача №{TaskNumber}:");
                Console.WriteLine(item.GetFullInfo());
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
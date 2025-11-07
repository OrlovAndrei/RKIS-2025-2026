namespace TodoList
{
    public class DoneCommand : ICommand
    {
        public int TaskNumber { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            try
            {
                var item = TodoList.GetItem(TaskNumber);
                item.MarkDone();
                Console.WriteLine($"Задача №{TaskNumber} отмечена как выполненная");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
namespace TodoList
{
    public class DeleteCommand : ICommand
    {
        public int TaskNumber { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            try
            {
                TodoList.Delete(TaskNumber);
                Console.WriteLine($"Задача №{TaskNumber} удалена");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
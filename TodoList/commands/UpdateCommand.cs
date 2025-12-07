namespace TodoList.commands
{
    public class UpdateCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int Index { get; set; }
        public string Text { get; set; }

        public void Execute()
        {
            TodoList[Index].UpdateText(Text);
            Console.WriteLine($"Задача #{Index + 1} обновлена.");
        }
    }
}

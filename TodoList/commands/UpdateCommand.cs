namespace TodoList.commands
{
    public class UpdateCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int Index { get; set; }
        public string Text { get; set; }
        
        private string oldText;

        public void Execute()
        {
            oldText = TodoList[Index].Text;
            TodoList[Index].UpdateText(Text);
            Console.WriteLine($"Задача #{Index + 1} обновлена.");
        }

        public void Unexecute()
        {
            if (!string.IsNullOrEmpty(oldText))
            {
                TodoList[Index].UpdateText(oldText);
                Console.WriteLine("Обновление задачи отменено");
            }
        }
    }
}

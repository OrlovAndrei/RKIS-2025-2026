namespace TodoList
{
    public class UpdateCommand : ICommand
    {
        public int TaskNumber { get; set; }
        public string NewText { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            try
            {
                var item = TodoList.GetItem(TaskNumber);
                string newText = NewText;

                if (newText.StartsWith("\"") && newText.EndsWith("\""))
                {
                    newText = newText.Substring(1, newText.Length - 2);
                }

                string oldText = item.Text;
                item.UpdateText(newText);
                Console.WriteLine($"Задача обновлена: \nБыло: №{TaskNumber} \"{oldText}\" \nСтало: №{TaskNumber} \"{newText}\"");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
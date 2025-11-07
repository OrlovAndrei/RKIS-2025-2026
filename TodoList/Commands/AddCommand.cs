namespace TodoList.Commands
{
    public class AddCommand : ICommand
    {
        public string Text { get; set; }
        public bool IsMultiline { get; set; }
        public string[] Flags { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            if (TodoList == null)
            {
                Console.WriteLine("Ошибка: список задач не инициализирован.");
                return;
            }

            TodoList.AddTask(Text, Flags ?? Array.Empty<string>());
        }
    }
}

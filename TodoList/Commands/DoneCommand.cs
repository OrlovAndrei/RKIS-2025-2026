namespace TodoList
{
    public class DoneCommand : ICommand
    {
        public string Arg { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            if (TodoList == null)
            {
                Console.WriteLine("Ошибка: список задач не инициализирован.");
                return;
            }

            TodoList.MarkTaskDone(Arg);
        }
    }
}

namespace TodoList
{
    public class DoneCommand : BaseCommand
    {
        public int Index { get; set; }

        public DoneCommand(TodoList todoList, int index) : base(todoList)
        {
            Index = index;
        }

        public override void Execute()
        {
            TodoItem item = TodoList.GetItem(Index);
            if (item == null)
                System.Console.WriteLine("Задача с таким индексом не найдена.");
            else
            {
                item.MarkDone();
                System.Console.WriteLine("Готово.");
            }
        }
    }
}
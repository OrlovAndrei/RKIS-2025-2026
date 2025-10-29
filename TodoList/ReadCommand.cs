namespace TodoList
{
    public class ReadCommand : BaseCommand
    {
        public int Index { get; set; }

        public ReadCommand(TodoList todoList, int index) : base(todoList)
        {
            Index = index;
        }

        public override void Execute()
        {
            TodoItem item = TodoList.GetItem(Index);
            if (item == null)
                System.Console.WriteLine("Задача с таким индексом не найдена.");
            else
                System.Console.WriteLine(item.GetFullInfo());
        }
    }
}

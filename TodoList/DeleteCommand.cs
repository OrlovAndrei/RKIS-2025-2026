namespace TodoList
{
    public class DeleteCommand : BaseCommand
    {
        public int Index { get; set; }

        public DeleteCommand(TodoList todoList, int index) : base(todoList)
        {
            Index = index;
        }

        public override void Execute()
        {
            if (!TodoList.Delete(Index))
                System.Console.WriteLine("Задача с таким индексом не найдена.");
            else
                System.Console.WriteLine("Удалено.");
        }
    }
}
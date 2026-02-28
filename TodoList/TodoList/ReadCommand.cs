using System;

class ReadCommand : ICommand
{
    public TodoList TodoList { get; set; }
    public int Index { get; set; }

    public void Execute()
    {
        var item = TodoList.GetItem(Index);
        if (item != null)
            Console.WriteLine(item.GetFullInfo());
    }
}

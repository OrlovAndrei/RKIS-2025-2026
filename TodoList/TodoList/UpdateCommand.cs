using System;

class UpdateCommand : ICommand
{
    public TodoList TodoList { get; set; }
    public int Index { get; set; }
    public string Text { get; set; }

    public void Execute()
    {
        var item = TodoList.GetItem(Index);
        if (item != null)
            item.UpdateText(Text);
    }
}

namespace TodoList
{
    public interface IUndo : ICommand
    {
        void Unexecute();
    }
}
namespace Todolist.Commands
{
    internal interface IUndo : ICommand
    {
        void Unexecute();
    }
}

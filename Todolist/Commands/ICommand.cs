namespace Todolist.Commands
{
    internal interface ICommand
    {
        void Execute();
        void Unexecute();
    }
}

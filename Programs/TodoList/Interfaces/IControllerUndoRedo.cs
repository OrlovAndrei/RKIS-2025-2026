namespace TodoList.Interfaces;

public interface IControllerUndoRedo
{
    Task Execute(ICommand command);
    Task Redo();
    Task Undo();
}
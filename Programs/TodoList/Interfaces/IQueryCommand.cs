namespace TodoList.Interfaces;

public interface IQueryCommand<TRequest> : ICommand
{
    new Task<TRequest> Execute();
}
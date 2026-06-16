namespace TodoList.Interfaces;

public interface ICommandWithUndo<T> : ICommand
{
    T? Value { get; }
    Task Execute(T value);
    new async Task Execute()
    {
        if (Value is null)
        {
            throw new SystemException();
        }
        await Execute(Value);
    }
    Task Unexecuted();
}
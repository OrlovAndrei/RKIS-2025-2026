namespace TodoList.Interfaces;

public interface ICurrentProfile
{
    Guid? Id { get; }
    Task Set(Guid newId);
    Task Clear();
}
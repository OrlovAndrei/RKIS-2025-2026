namespace Application.Interfaces;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task SaveChangesAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
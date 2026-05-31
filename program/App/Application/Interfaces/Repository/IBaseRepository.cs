namespace Application.Interfaces.Repository;

public interface IBaseRepository<T>
{
	Task AddAsync(T obj);
	Task UpdateAsync(T obj);
	Task DeleteAsync(Guid id);
	Task<T?> GetByIdAsync(Guid id);
}
namespace Domain.Interfaces;

public interface IBaseRepository<T>
{
	Task<int> AddAsync(T obj);
	Task<int> UpdateAsync(T obj);
	Task<int> DeleteAsync(Guid id);
	Task<T?> GetByIdAsync(Guid id);
	Task<IEnumerable<T>> GetAllAsync();
}
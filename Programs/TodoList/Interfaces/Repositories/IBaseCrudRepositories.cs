using System.Linq.Expressions;

namespace TodoList.Interfaces.Repositories;

public interface IBaseCrudRepositories<TValue, TId>
{
    Task AddAsync(TValue obj);
	Task UpdateAsync(TValue obj);
	Task DeleteAsync(TId id);
	Task<TValue?> GetByIdAsync(TId id);
    Task<IEnumerable<TValue>> FindAsync(Expression<Func<TValue, bool>> predicate);
    Task<TValue?> FindSingleAsync(Expression<Func<TValue, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<TValue, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TValue, bool>> predicate);
}
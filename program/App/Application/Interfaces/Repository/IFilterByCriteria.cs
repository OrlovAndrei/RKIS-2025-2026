using System.Linq.Expressions;

namespace Application.Interfaces.Repository;

public interface IFilterByCriteria<TValue>
{
    Task<IEnumerable<TValue>> FindAsync(Expression<Func<TValue, bool>> predicate);
    Task<TValue?> FindSingleAsync(Expression<Func<TValue, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<TValue, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TValue, bool>> predicate);
}
namespace Domain.Interfaces;

public interface IFilterByCriteria<TValue, TCriteria>
{
    Task<IEnumerable<TValue>> FindAsync(TCriteria profileCriteria);
    Task<TValue?> FindSingleAsync(TCriteria profileCriteria);
    Task<bool> ExistsAsync(TCriteria profileCriteria);
    Task<int> CountAsync(TCriteria profileCriteria);
}
using Domain.Entities.ProfileEntity;

namespace Application.Interfaces.Repository;

public interface IProfileRepository : IBaseRepository<Profile>, IFilterByCriteria<Profile>
{
	Task<IEnumerable<Profile>> GetAllAsync();
}
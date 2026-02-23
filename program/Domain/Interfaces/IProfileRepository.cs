using Domain.Entities.ProfileEntity;
using Domain.Specifications;

namespace Domain.Interfaces;

public interface IProfileRepository : IBaseRepository<Profile>, IFilterByCriteria<Profile, ProfileCriteria>
{
    
}
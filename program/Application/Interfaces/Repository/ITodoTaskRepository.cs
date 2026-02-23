using Application.Specifications;
using Domain.Entities.TaskEntity;

namespace Application.Interfaces.Repository;

public interface ITodoTaskRepository : IBaseRepository<TodoTask>, IFilterByCriteria<TodoTask, TaskCriteria>
{
    
}
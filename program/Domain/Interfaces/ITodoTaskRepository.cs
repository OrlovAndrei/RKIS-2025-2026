using Domain.Entities.TaskEntity;
using Domain.Specifications;

namespace Domain.Interfaces;

public interface ITodoTaskRepository : IBaseRepository<TodoTask>, IFilterByCriteria<TodoTask, TaskCriteria>
{
    
}
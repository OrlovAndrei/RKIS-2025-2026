using TodoList.Data;

namespace TodoList.Commands
{
    public interface IRepositoryCommand
    {
        void SetRepositories(IProfileRepository profileRepository, ITodoRepository todoRepository);
    }
}

using TodoList.Interfaces;

namespace TodoList.Infrastructure;

public class CurrentProfile : ICurrentProfile
{
    public Guid? Id { get; private set; }

	public async Task Clear()
	{
		Id = null;
	}

	public async Task Set(Guid newId)
    {
        Id = newId;
    }
    
}
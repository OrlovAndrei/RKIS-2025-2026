using Application.Interfaces;

namespace Infrastructure;

public class UserContext : IUserContext
{
	public Guid? UserId { get; private set; }
	public UserContext(Guid? idProfile = null)
	{
		UserId = idProfile;
	}
	public void Clear()
	{
		UserId = Guid.Empty;
	}

	public void Set(Guid? userId)
	{
		UserId = userId;
	}
}
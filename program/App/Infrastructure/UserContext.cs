using Application.Interfaces;

namespace Infrastructure;

public class UserContext : IUserContext
{
	public Guid? UserId { get; private set; } = null;
	public UserContext(Guid? idProfile = null)
	{
		if (UserId is not null && UserId == idProfile)
		{
			throw new ArgumentException(message: "You cannot re-enter a profile that is already active.");
		}
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
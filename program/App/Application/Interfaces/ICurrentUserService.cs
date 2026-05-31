namespace Application.Interfaces;

public interface IUserContext
{
	Guid? UserId { get; }
	void Set(Guid? userId);
	void Clear();
}
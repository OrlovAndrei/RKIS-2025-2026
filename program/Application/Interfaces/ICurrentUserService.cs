namespace Application.Interfaces;

public interface ICurrentUserService
{
	Guid? UserId { get; }
	void Set(Guid? userId);
	void Clear();
}
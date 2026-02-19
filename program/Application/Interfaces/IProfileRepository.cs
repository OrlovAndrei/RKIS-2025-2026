using Domain;

namespace Application.Interfaces;

public interface IProfileRepository
{
	void Add(Profile profile);
	void Update(Profile profile);
	void Delete(Guid id);
	Profile? GetById(Guid id);
	IEnumerable<Profile> GetAll();
}
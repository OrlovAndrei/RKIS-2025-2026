using Application.Dto;
using Application.Interfaces;
using Domain.Interfaces;
using Domain;

namespace Application.UseCase.ProfileUseCases;

public class ProfileUseCase(IProfileRepository repository, IPasswordHashed passwordHashed)
{
	private readonly IProfileRepository _repo = repository;
	private readonly IPasswordHashed _passwordHashed = passwordHashed;
	public void Add(ProfileDto.ProfileCreateDto profileCreate)
	{
		_repo.AddAsync(profileCreate.FromCreateDto(passwordHashed: _passwordHashed));
	}
	public void Delate(Guid profileId)
	{
		_repo.DeleteAsync(profileId);
	}
	public async Task<IEnumerable<ProfileDto.ProfileShortDto>> GetAllProfileShorts()
	{
		IEnumerable<Profile> profiles = await _repo.GetAllAsync();
		IEnumerable<ProfileDto.ProfileShortDto> profileShorts = profiles.Select(p => p.ToShortDto()).ToArray();
		return profileShorts;
	}
	public async Task<ICurrentUserService> CurrentUserServiceTest(Guid profileId, string password, ICurrentUserService currentUserService)
	{
		Profile? profile = (await _repo.GetAllAsync()).FirstOrDefault(p => p.ProfileId == profileId);
		if (profile is null)
		{
			profile = new(
				firstName: "TestFirstProfile",
				lastName: "Если ты это видишь то перекрестись.",
				dateOfBirth: new DateTime(year: 2007, month: 04, day: 20),
				password: password,
				hashed: _passwordHashed);
			Add(profile.ToCreateDto());
		}
		if (_passwordHashed.VerifyAsync(password: password, hashedPassword: profile.PasswordHash, createAt: profile.CreatedAt).Result)
		{
			currentUserService.Set(profile.ProfileId);
		}
		return currentUserService;
	}
}
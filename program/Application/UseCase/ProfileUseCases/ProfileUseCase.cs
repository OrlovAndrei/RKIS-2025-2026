using Application.Dto;
using Application.Interfaces;
using Domain;

namespace Application.UseCase.ProfileUseCases;

public class ProfileUseCase(IProfileRepository repository, IPasswordHashed passwordHashed)
{
    private readonly IProfileRepository _repo = repository;
    private readonly IPasswordHashed _passwordHashed = passwordHashed;
    public void Add(ProfileDto.ProfileCreateDto profileCreate)
    {
        _repo.Add(profileCreate.FromCreateDto(passwordHashed: _passwordHashed));
    }
    public void Delate(Guid profileId)
    {
        _repo.Delete(profileId);
    }
    public IEnumerable<ProfileDto.ProfileShortDto> GetAllProfileShorts()
    {
        IEnumerable<Profile> profiles = _repo.GetAll();
        IEnumerable<ProfileDto.ProfileShortDto> profileShorts = profiles.Select(p => p.ToShortDto()).ToArray();
        return profileShorts;
    }
    public ICurrentUserService CurrentUserServiceTest(Guid profileId, string password, ICurrentUserService currentUserService)
    {
        Profile? profile = _repo.GetAll().FirstOrDefault(p => p.ProfileId == profileId);
        if (profile is null)
        {
            profile = new(
                firstName: "TestFirstProfile",
                lastName: "Если ты это видишь то перекрестись.",
                dateOfBirth: new DateTime(year: 2007, month: 04, day: 20),
                passwordHash: password);
            Add(profile.ToCreateDto());
        }
        if (_passwordHashed.Verify(password: password, hashedPassword: profile.PasswordHash))
        {
            currentUserService.Set(profile.ProfileId);
        }
        return currentUserService;
    }
}
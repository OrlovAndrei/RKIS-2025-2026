using Application.Dto;
using Application.Interfaces;
using Domain;
using Domain.Interfaces;

namespace Application.UseCase.ProfileUseCases;

public class AddNewProfileUseCase : IUndoRedo
{
    private readonly IProfileRepository _repo;
    private readonly IPasswordHashed _hashed;
    private readonly ProfileDto.ProfileCreateDto _profileCreate;
    private readonly Profile _newProfile;
    public AddNewProfileUseCase(
        IProfileRepository repository,
        IPasswordHashed hashed,
        ProfileDto.ProfileCreateDto profileCreate)
    {
        _repo = repository;
        _hashed = hashed;
        _profileCreate = profileCreate;
        _newProfile = _profileCreate.FromCreateDto(passwordHashed: _hashed);
    }
    public async Task<int> Execute()
    {
        return await _repo.AddAsync(_newProfile);
    }
    public async Task<int> Undo()
    {
        return await _repo.DeleteAsync(_newProfile.ProfileId);
    }
}
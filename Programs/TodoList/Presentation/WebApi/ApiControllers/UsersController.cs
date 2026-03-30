using Microsoft.AspNetCore.Mvc;
using TodoList.Dto;
using TodoList.Entity;
using TodoList.Interfaces;
using TodoList.UseCases.ProfileUseCases;
using TodoList.UseCases.ProfileUseCases.Query;

namespace TodoList.Presentation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AddProfileUseCase _addProfileUseCase;
    private readonly GetAllProfileUseCase _getAllProfileUseCase;
    private IHasher _hasher;
    private IClock _clock;
    public UsersController(
        AddProfileUseCase addProfileUseCase,
        GetAllProfileUseCase getAllProfileUseCase,
        IHasher hasher,
        IClock clock
    )
    {
        _getAllProfileUseCase = getAllProfileUseCase;
        _addProfileUseCase = addProfileUseCase;
        _hasher = hasher;
        _clock = clock;
    }
    [HttpPost]
    public async Task<IActionResult> CreateProfile(ProfileDto.Create create)
    {
        Profile profile = new(
            login: create.Login,
            birthYear: create.DateOfBirth,
            password: create.Password,
            clock: _clock,
            hasher: _hasher,
            firstName: create.FirstName,
            lastName: create.LastName
        );
        await _addProfileUseCase.Execute(profile);
        return Ok();
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Profile>>> GetAllProfile()
    {
        var profiles = await _getAllProfileUseCase.Execute();
        if (profiles is null)
        {
            return NotFound();
        }
        return Ok(profiles);
    }
}
using Application.Interfaces;
using Domain;

namespace Application.Dto;

public static class ProfileDto
{
    public record ProfileDetailsDto(
        Guid ProfileId,
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        DateTime CreatedAt);
    public static ProfileDetailsDto ToDetailsDto(this Profile profile) => new(
        ProfileId: profile.ProfileId,
        FirstName: profile.FirstName,
        LastName: profile.LastName,
        DateOfBirth: profile.DateOfBirth,
        CreatedAt: profile.CreatedAt
    );
    public record ProfileShortDto(
        Guid ProfileId,
        string FirstName,
        string LastName);
    public static ProfileShortDto ToShortDto(this Profile profile) => new(
        ProfileId: profile.ProfileId,
        FirstName: profile.FirstName,
        LastName: profile.LastName
    );
    public record ProfileUpdateDto(
        string FirstName,
        string LastName,
        DateTime DateOfBirth);
    public static ProfileUpdateDto ToUpdateDto(this Profile profile) => new(
        FirstName: profile.FirstName,
        LastName: profile.LastName,
        DateOfBirth: profile.DateOfBirth
    );
    public record ProfileCreateDto(
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        string PasswordHash);
    public static ProfileCreateDto ToCreateDto(this Profile profile) => new(
        FirstName: profile.FirstName,
        LastName: profile.LastName,
        DateOfBirth: profile.DateOfBirth,
        PasswordHash: profile.PasswordHash
    );
    public static Profile FromCreateDto(
        this ProfileCreateDto profileCreateDto, 
        IPasswordHashed passwordHashed) => new(
        firstName: profileCreateDto.FirstName,
        lastName: profileCreateDto.LastName,
        dateOfBirth: profileCreateDto.DateOfBirth,
        passwordHash: passwordHashed.Hashed(profileCreateDto.PasswordHash)
    );
}
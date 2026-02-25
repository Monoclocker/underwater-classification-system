using FluentResults;
using Microsoft.EntityFrameworkCore;
using ProfilesService.Domain.Entities;
using ProfilesService.Errors.HandlersErrors;
using ProfilesService.Infrastructure.Database;
using ProfilesService.Infrastructure.Database.Extensions;

namespace ProfilesService.Handlers.Commands.CreateUserProfile;

internal sealed class CreateUserProfileCommandHandler : ICommandHandler<CreateUserProfileCommand>
{
    private readonly ProfilesDbContext _context;
    
    public CreateUserProfileCommandHandler(ProfilesDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> HandleAsync(CreateUserProfileCommand command)
    {
        UserProfile newProfile = UserProfile.Create(command.UserId, command.GetPersonalInformation());

        try
        {
            _context.Profiles.Add(newProfile);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (DbUpdateException ex) when (ex.IsUniqueViolation())
        {
            return Result.Fail(new EntityAlreadyExistsError());
        }
    }
}
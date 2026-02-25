using ProfilesService.Domain.ValueObjects;

namespace ProfilesService.Handlers.Commands.CreateUserProfile;

public sealed record CreateUserProfileCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string ShownUsername) : BaseCommand
{
    internal UserPersonalInformation GetPersonalInformation()
    {
        return new(
            ShownUsername,
            FirstName,
            LastName);
    }
}
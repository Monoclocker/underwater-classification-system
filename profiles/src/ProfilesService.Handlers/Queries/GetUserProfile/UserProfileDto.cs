namespace ProfilesService.Handlers.Queries.GetUserProfile;

public sealed record UserProfileDto(
    string? ImageLink,
    string ShownName,
    string FirstName,
    string LastName);
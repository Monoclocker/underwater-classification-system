namespace ProfilesService.Domain.ValueObjects;

public sealed record UserPersonalInformation(
    string ShownUserName,
    string FirstName,
    string LastName,
    string Email);
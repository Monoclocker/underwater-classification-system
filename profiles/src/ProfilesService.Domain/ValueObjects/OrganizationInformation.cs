namespace ProfilesService.Domain.ValueObjects;

public sealed record OrganizationInformation(
    string OrganizationName,
    string Email,
    bool IsPublic);
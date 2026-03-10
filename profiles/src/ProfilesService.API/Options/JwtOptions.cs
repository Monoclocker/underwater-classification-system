namespace ProfilesService.API.Options;

public sealed class JwtOptions
{
    public required string Audience { get; init; }
    public required string Issuer { get; init; }
}
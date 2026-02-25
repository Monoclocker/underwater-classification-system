using FluentResults;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using ProfilesService.Domain.Interfaces;

namespace ProfilesService.Infrastructure.EmailService;

internal sealed class KeycloakAdministrationEmailService : IEmailService
{
    private readonly IKeycloakUserClient _client;

    public KeycloakAdministrationEmailService(IKeycloakUserClient client)
    {
        _client = client;
    }

    public async Task<Result> UpdateEmailAsync(Guid userId, string newEmail)
    {
        try
        {
            await _client.UpdateUserAsync("test", userId.ToString(), new UserRepresentation { Email = newEmail });
            return Result.Ok();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Result> RequestVerificationEmailAsync(Guid userId)
    {
        try
        {
            await _client.SendVerifyEmailAsync("test", userId.ToString());

            return Result.Ok();
        }
        catch
        {
            throw;
        }
    }
}
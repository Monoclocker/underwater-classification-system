using FluentResults;
using Microsoft.AspNetCore.Mvc;
using ProfilesService.API.Extensions;
using ProfilesService.Handlers.Commands;
using ProfilesService.Handlers.Commands.CreateUserProfile;

namespace ProfilesService.API.Endpoints;

public sealed record CreateUserProfileRequest(
    string ShownName,
    string FirstName,
    string LastName);

public sealed class CreateUserProfileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/profiles", async (
                HttpContext context,
                [FromBody] CreateUserProfileRequest request,
                ICommandHandler<CreateUserProfileCommand, Result> handler) =>
        {
            Guid userId = context.User.GetId();

            var command = new CreateUserProfileCommand(userId, 
                ShownUsername: request.ShownName, 
                FirstName: request.FirstName,
                LastName: request.LastName);
            
            Result result = await handler.HandleAsync(command);

            if (result.IsFailed)
            {
                var response = ApiResponse.FromResult(result);
                return Results.BadRequest(response);
            }
            
            return Results.Ok();
        })
        .RequireAuthorization();
    }
}
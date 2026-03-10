using Microsoft.AspNetCore.Mvc;
using ProfilesService.API.Extensions;
using ProfilesService.Handlers.Commands;
using ProfilesService.Handlers.Queries;
using ProfilesService.Handlers.Queries.GetUserProfile;

namespace ProfilesService.API.Endpoints;

public sealed class GetUserProfileEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    { 
        builder.MapGet("/api/profiles/me", async (
                HttpContext context,
                [FromServices] IQueryHandler<GetUserProfileQuery, UserProfileDto?> handler) =>
        {
            Guid userId = context.User.GetId();

            UserProfileDto? response = await handler.HandleAsync(new GetUserProfileQuery(userId));

            if (response is null)
                return Results.NotFound();

            return Results.Ok(response);
        })
        .RequireAuthorization();
    }
}
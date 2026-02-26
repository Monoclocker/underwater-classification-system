namespace ProfilesService.Handlers.Queries.GetUserProfile;

public sealed record GetUserProfileQuery(Guid UserId) : BaseQuery;
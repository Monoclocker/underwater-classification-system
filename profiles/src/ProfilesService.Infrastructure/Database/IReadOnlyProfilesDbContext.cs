using ProfilesService.Domain.Entities;

namespace ProfilesService.Infrastructure.Database;

public interface IReadOnlyProfilesDbContext
{
    public IQueryable<UserProfile> ReadonlyProfiles { get; }
    public IQueryable<Organization> ReadonlyOrganizations { get; }
}
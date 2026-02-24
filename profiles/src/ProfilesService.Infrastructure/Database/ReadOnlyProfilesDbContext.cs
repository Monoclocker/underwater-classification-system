using Microsoft.EntityFrameworkCore;
using ProfilesService.Domain.Entities;

namespace ProfilesService.Infrastructure.Database;

internal sealed class ReadOnlyProfilesDbContext(DbContextOptions<ReadWriteProfilesDbContext> options) 
    : BaseProfilesDbContext(options), IReadOnlyProfilesDbContext
{
    public IQueryable<UserProfile> ReadonlyProfiles => Profiles.AsNoTracking();
    public IQueryable<Organization> ReadonlyOrganizations => Organizations.AsNoTracking();
}
using Microsoft.EntityFrameworkCore;
using ProfilesService.Domain.Entities;

namespace ProfilesService.Infrastructure.Database;

public abstract class BaseProfilesDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Organization> Organizations { get; init; }
    public DbSet<UserProfile> Profiles { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
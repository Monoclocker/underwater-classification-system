using Microsoft.EntityFrameworkCore;
using ProfilesService.Domain.Entities;

namespace ProfilesService.Infrastructure.Database;

public sealed class ReadWriteProfilesDbContext(DbContextOptions<ReadWriteProfilesDbContext> options)
    : BaseProfilesDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Organization>()
            .Navigation(x => x.OrganizationMembers)
            .AutoInclude();

        modelBuilder.Entity<OrganizationMember>()
            .Navigation(x => x.MemberNavigation)
            .AutoInclude();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProfilesService.Infrastructure.Database.Design;

internal class DesignTimeContextFactory : IDesignTimeDbContextFactory<ProfilesDbContext>
{
    public ProfilesDbContext CreateDbContext(string[] args)
    {
        string connectionString = args[0];

        DbContextOptions<ProfilesDbContext> options = new DbContextOptionsBuilder<ProfilesDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        
        return new ProfilesDbContext(options);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProfilesService.Infrastructure.Database.Design;

internal sealed class DesignTimeContextFactory : IDesignTimeDbContextFactory<ProfilesDbContext>
{
    public ProfilesDbContext CreateDbContext(string[] args)
    {  
        DbContextOptions<ProfilesDbContext> options = new DbContextOptionsBuilder<ProfilesDbContext>()
            .UseNpgsql(string.Empty)
            .Options;
        
        return new ProfilesDbContext(options);
    }
}
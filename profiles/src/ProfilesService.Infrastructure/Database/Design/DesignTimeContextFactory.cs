using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProfilesService.Infrastructure.Database.Design;

internal class DesignTimeContextFactory : IDesignTimeDbContextFactory<DesignTimeContext>
{
    public DesignTimeContext CreateDbContext(string[] args)
    {
        string connectionString = args[0];

        DbContextOptions<DesignTimeContext> optionsBuilder = new DbContextOptionsBuilder<DesignTimeContext>()
            .UseNpgsql(connectionString)
            .Options;
        
        return new DesignTimeContext(optionsBuilder);
    }
}
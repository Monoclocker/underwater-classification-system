using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProfilesService.Infrastructure.Database.Extensions;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddDatabase(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Database")
                ?? throw new ArgumentException("Database connection string was not provided");

            services.AddDbContext<ProfilesDbContext>(
                options => options.UseNpgsql(connectionString));
        } 
    }
}
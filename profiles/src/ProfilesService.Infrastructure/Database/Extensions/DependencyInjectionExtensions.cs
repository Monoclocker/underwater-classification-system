using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using ProfilesService.Infrastructure.Database.Options;

namespace ProfilesService.Infrastructure.Database.Extensions;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddDatabase(DatabaseConfigurationOptions configurationOptions)
        {
            string connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = configurationOptions.Host,
                Port = configurationOptions.Port,
                Database = configurationOptions.DatabaseName,
                Username = configurationOptions.Username,
                Password = configurationOptions.Password,
            }.ConnectionString;
            
            services.AddDbContext<ProfilesDbContext>(
                options => options.UseNpgsql(connectionString));
        } 
    }
}
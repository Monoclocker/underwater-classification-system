using DevicesService.Domain.Interfaces;
using DevicesService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace DevicesService.Infrastructure;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddDatabase(DatabaseConnectionOptions configuration)
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder()
            {
                Host = configuration.Host,
                Port = configuration.Port,
                Username = configuration.Username,
                Password = configuration.Password,
                Database = configuration.Database
            };

            services.AddDbContext<IApplicationDatabaseContext, ApplicationDatabaseContext>(options =>
            {
                options.UseNpgsql(builder.ConnectionString);
            });
        }
    }
}
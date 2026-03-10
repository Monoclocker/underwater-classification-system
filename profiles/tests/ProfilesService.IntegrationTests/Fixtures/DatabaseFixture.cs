using Microsoft.EntityFrameworkCore;
using ProfilesService.Infrastructure.Database;
using Testcontainers.PostgreSql;

namespace ProfilesService.IntegrationTests.Fixtures;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:latest")
        .WithDatabase("profiles_test")
        .WithUsername("test")
        .WithPassword("test")
        .Build();


    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        
        await using var context = BuildContext();

        await context.Database.MigrateAsync();
    }
    
    public ProfilesDbContext BuildContext()
    {
        var options = new DbContextOptionsBuilder<ProfilesDbContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;
        
        return new ProfilesDbContext(options);
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
    }
}
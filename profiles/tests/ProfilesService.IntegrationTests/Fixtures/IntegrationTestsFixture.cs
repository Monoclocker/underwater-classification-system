namespace ProfilesService.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(IntegrationTestsFixture))]
public sealed class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture>;

public sealed class IntegrationTestsFixture : IAsyncLifetime
{
    public DatabaseFixture DatabaseFixture { get; } = new();
    public DistributedCacheFixture DistributedCacheFixture { get; } = new();
    
    public async Task InitializeAsync()
    {
        await DatabaseFixture.InitializeAsync();
        await DistributedCacheFixture.InitializeAsync();
    }

    public async Task DisposeAsync()
    {
        await DatabaseFixture.DisposeAsync();
        await DistributedCacheFixture.DisposeAsync();
    }
}
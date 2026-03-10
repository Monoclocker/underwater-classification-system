using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using ProfilesService.Domain.Entities;
using ProfilesService.Domain.ValueObjects;
using ProfilesService.Handlers.Queries.GetUserProfile;
using ProfilesService.Infrastructure.Database;
using ProfilesService.IntegrationTests.Fixtures;

namespace ProfilesService.IntegrationTests.Handlers;

[Collection(nameof(IntegrationTestsFixture))]
public sealed class GetUserProfileHandlerTests : IAsyncDisposable
{
    private readonly ProfilesDbContext _context;
    private readonly RedisCache _cache;
    private readonly GetUserProfileQueryHandler _handler;
    
    public GetUserProfileHandlerTests(IntegrationTestsFixture fixtureCollection)
    {
        _context = fixtureCollection.DatabaseFixture.BuildContext();
        _cache = fixtureCollection.DistributedCacheFixture.GetDistributedCache();
        _handler = new GetUserProfileQueryHandler(_cache, _context);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnUserProfileFromCache_IfItExists()
    {
        GetUserProfileQuery query = new GetUserProfileQuery(Guid.NewGuid());
        UserProfileDto cachedProfile = new UserProfileDto(
            null,
            "test",
            "Тест",
            "test");
        
        await _cache.SetAsync(query.UserId.ToString(), JsonSerializer.SerializeToUtf8Bytes(cachedProfile));
        
        var result = await _handler.HandleAsync(query);
        
        Assert.Equal(cachedProfile, result);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnUserProfileFromDatabaseAndUpdateCache_IfItNotExistsInCache()
    {
        GetUserProfileQuery query = new GetUserProfileQuery(Guid.NewGuid());

        UserProfile profile = UserProfile.Create(
            query.UserId,
            new UserPersonalInformation("test", "test", "test"));
        
        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        UserProfileDto? result = await _handler.HandleAsync(query);
        UserProfileDto expectedResult = new UserProfileDto(
            null,
            profile.PersonalInformation.ShownUserName,
            profile.PersonalInformation.FirstName,
            profile.PersonalInformation.LastName);
        UserProfileDto? cachedProfile = JsonSerializer.Deserialize<UserProfileDto>(
            await _cache.GetAsync(query.UserId.ToString()));
        
        Assert.NotNull(cachedProfile);
        Assert.Equal(cachedProfile, result);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNull_IfProfileDoesNotExistsInCacheAndDatabase()
    {
        GetUserProfileQuery query = new GetUserProfileQuery(Guid.NewGuid());
        
        UserProfileDto? result = await _handler.HandleAsync(query);
        
        byte[]? cachedObjectBytes = await _cache.GetAsync(query.UserId.ToString());
        
        Assert.Null(result);
        Assert.Null(cachedObjectBytes);
    }
    
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
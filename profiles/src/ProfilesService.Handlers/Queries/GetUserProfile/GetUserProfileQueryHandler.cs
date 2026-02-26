using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ProfilesService.Domain.Entities;
using ProfilesService.Infrastructure.Cache;
using ProfilesService.Infrastructure.Database;

namespace ProfilesService.Handlers.Queries.GetUserProfile;

internal sealed class  GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, UserProfileDto?>
{
    private readonly IDistributedCache _cache;
    private readonly ProfilesDbContext _context;
    
    public GetUserProfileQueryHandler(IDistributedCache cache, ProfilesDbContext context)
    {
        _cache = cache;
        _context = context;
    }

    public async Task<UserProfileDto?> HandleAsync(GetUserProfileQuery query)
    {
        UserProfileDto? userProfileDto = await _cache.GetObjectAsync<UserProfileDto>(query.UserId.ToString());

        if (userProfileDto is not null)
            return userProfileDto;
        
        UserProfile? userProfile = await _context
            .Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.UserId);

        if (userProfile is not null)
        {
            (string shownUserName, string firstName, string lastName) = userProfile.PersonalInformation;
        
            var profile = new UserProfileDto(
                userProfile.Image?.Link,
                shownUserName, 
                firstName,
                lastName);
            
            await _cache.SetObjectAsync(query.UserId.ToString(), profile);

            return profile;
        }

        return null;
    }
}
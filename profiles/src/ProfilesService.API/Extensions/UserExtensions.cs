using System.Security.Claims;

namespace ProfilesService.API.Extensions;

public static class UserExtensions
{
    extension(ClaimsPrincipal user)
    {
        public Guid GetId()
        {
            string requiredClaimValue = user.FindFirstValue("sub")
                                        ?? throw new InvalidOperationException("User ID claim not found");
            
            return Guid.Parse(requiredClaimValue);
        } 
    }
}
using System.Security.Claims;

namespace DevicesService.API.Extensions;

public static class UserIdentityExtensions
{
    extension(ClaimsPrincipal identity)
    {
        public Guid GetUserId()
        {
            var ownerId = identity.FindFirst(x => x.Type == ClaimTypes.PrimarySid)?.Value;
            
            if (ownerId is null) throw new ArgumentException("Owner ID claim not found");
            
            return Guid.Parse(ownerId);
        } 
    }
}
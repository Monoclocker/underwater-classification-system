using Microsoft.EntityFrameworkCore;

namespace ProfilesService.Infrastructure.Database.Design;

internal class DesignTimeContext(DbContextOptions<DesignTimeContext> options) : BaseProfilesDbContext(options);
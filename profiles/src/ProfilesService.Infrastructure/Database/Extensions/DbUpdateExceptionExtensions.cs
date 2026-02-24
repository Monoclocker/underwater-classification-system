using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ProfilesService.Infrastructure.Database.Extensions;

public static class DbUpdateExceptionExtensions
{
    extension(DbUpdateException ex)
    {
        public bool IsUniqueViolation()
        {
            return ex.InnerException is NpgsqlException { ErrorCode: 23505 };
        }
    } 
}
using DevicesService.Domain.Interfaces;
using DevicesService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DevicesService.Infrastructure.Database;

public sealed class ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) 
    : DbContext(options), IApplicationDatabaseContext
{
    public DbSet<Device> Devices { get; init; }
}
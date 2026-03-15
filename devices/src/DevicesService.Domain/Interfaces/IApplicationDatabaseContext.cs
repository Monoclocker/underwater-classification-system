using DevicesService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DevicesService.Domain.Interfaces;

public interface IApplicationDatabaseContext
{
    DbSet<Device> Devices { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
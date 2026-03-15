using DevicesService.Domain.Interfaces;
using DevicesService.Domain.Models;
using DevicesService.Domain.Services.DTO;
using DevicesService.Domain.Extensions;
using FluentResults;
using Microsoft.Extensions.Caching.Distributed;

namespace DevicesService.Domain.Services;

internal sealed class DevicesService : IDevicesService
{
    private readonly IApplicationDatabaseContext _context;
    private readonly IDistributedCache _cache;
    
    public DevicesService(IApplicationDatabaseContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<DeviceDto?> GetDeviceAsync(Guid deviceId)
    {
        var device = await _cache.GetAsync<DeviceDto>($"device_{deviceId}");

        if (device is not null) return device;

        var storedDevice = await FetchDeviceAsync(deviceId);
        
        if (storedDevice is not null) return DeviceDto.FromModel(storedDevice);

        return null;
    }
    
    public async Task<DeviceFocus?> GetDeviceFocusAsync(Guid deviceId)
    {
        var device = await GetDeviceAsync(deviceId);
        
        if (device is null) return null ;
        
        return device.DeviceFocus;
    }

    public async Task<Guid> CreateDeviceAsync(Guid ownerId, DeviceFocus? focus)
    {
        Device newDevice = new Device(ownerId, focus?.X , focus?.Y);
        
        _context.Devices.Add(newDevice);

        await _context.SaveChangesAsync();

        return newDevice.Id;
    }

    public async Task<Result> UpdateDeviceFocusAsync(Guid userId, Guid deviceId, DeviceFocus focus)
    {
        var device = await FetchDeviceAsync(deviceId);
        
        if (device is null) return Result.Fail("Device not found");
        
        await _context.SaveChangesAsync();

        await _cache.SetAsync($"device_{deviceId}", DeviceDto.FromModel(device));
        
        return Result.Ok();
    }

    public async Task<Result> RemoveDeviceAsync(Guid userId, Guid deviceId)
    {
        await _cache.RemoveAsync($"device_{deviceId}");
        
        var device = await FetchDeviceAsync(deviceId);
        
        if (device is null) return Result.Fail("Device not found");
        
        if (device.OwnerId != userId) return Result.Fail("Unauthorized");

        _context.Devices.Remove(device);
        
        await _context.SaveChangesAsync();
        
        return Result.Ok();
    }

    private ValueTask<Device?> FetchDeviceAsync(Guid deviceId) => _context.Devices.FindAsync(deviceId);
}

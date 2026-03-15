using DevicesService.Domain.Services.DTO;
using FluentResults;

namespace DevicesService.Domain.Services;

public interface IDevicesService
{
    Task<DeviceDto?> GetDeviceAsync(Guid deviceId);
    Task<DeviceFocus?> GetDeviceFocusAsync(Guid deviceId);
    Task<Guid> CreateDeviceAsync(Guid ownerId, DeviceFocus? focus);
    Task<Result> UpdateDeviceFocusAsync(Guid userId, Guid deviceId, DeviceFocus focus);
    Task<Result> RemoveDeviceAsync(Guid userId, Guid deviceId);
}
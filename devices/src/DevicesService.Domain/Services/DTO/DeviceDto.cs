using DevicesService.Domain.Models;

namespace DevicesService.Domain.Services.DTO;

public record DeviceDto(Guid Id, Guid OwnerId, DeviceFocus DeviceFocus)
{
    public static DeviceDto FromModel(Device device)
    {
        return new DeviceDto(device.Id, device.OwnerId, new DeviceFocus(device.FocusX, device.FocusY));
    }
}
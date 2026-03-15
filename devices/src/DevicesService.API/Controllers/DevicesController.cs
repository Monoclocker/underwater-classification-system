using DevicesService.API.Extensions;
using DevicesService.Domain.Services;
using DevicesService.Domain.Services.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevicesService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class DevicesController : ControllerBase
{
    private readonly IDevicesService _devicesService;
    
    public DevicesController(IDevicesService devicesService)
    {
        _devicesService = devicesService;
    }
    
    [HttpGet("{deviceId:guid}")]
    public async Task<IActionResult> GetDeviceAsync(Guid deviceId)
    {
        var device = await _devicesService.GetDeviceAsync(deviceId);

        if (device is null) return NotFound();
        
        return Ok(device);
    }

    [HttpGet("{deviceId:guid}/focus")]
    public async Task<IActionResult> GetDeviceFocusAsync(Guid deviceId)
    {
        var focus = await _devicesService.GetDeviceFocusAsync(deviceId);
        
        if (focus is null) return NotFound();
        
        return Ok(focus);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDeviceAsync([FromBody] DeviceFocus focus)
    {
        var ownerId = User.GetUserId();
        
        var deviceId = await _devicesService.CreateDeviceAsync(ownerId, focus);
        
        return Ok(deviceId); //TODO: Represent it as JSON
    }

    [HttpPut("{deviceId:guid}")]
    public async Task<IActionResult> UpdateDeviceFocusAsync(Guid deviceId, [FromBody] DeviceFocus focus)
    {
        var ownerId = User.GetUserId();

        var result = await _devicesService.UpdateDeviceFocusAsync(ownerId, deviceId, focus);

        if (result.IsFailed) return BadRequest(); //TODO: Add error object
        
        return Ok();
    }

    [HttpDelete("{deviceId:guid}")]
    public async Task<IActionResult> RemoveDeviceAsync(Guid deviceId)
    {
        var ownerId = User.GetUserId();

        var result = await _devicesService.RemoveDeviceAsync(ownerId, deviceId);

        if (result.IsFailed) return BadRequest(); //TODO: Add error object
        
        return Ok();
    }
}
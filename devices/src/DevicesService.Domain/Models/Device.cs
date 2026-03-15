namespace DevicesService.Domain.Models;

public sealed class Device
{
    private const double DefaultFocusX = 0.0;
    private const double DefaultFocusY = 0.0;
    
    public Guid Id { get; private init; }
    public Guid OwnerId { get; private init; }
    public double FocusX { get; private set; }
    public double FocusY { get; private set; }
    
    private Device() {}
    
    public Device(Guid ownerId, double? focusX, double? focusY)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        FocusX = focusX ?? DefaultFocusX;
        FocusY = focusY ?? DefaultFocusY;
    }

    public void UpdateFocus(double focusX, double focusY)
    {
        FocusX = focusX;
        FocusY = focusY;
    }
}
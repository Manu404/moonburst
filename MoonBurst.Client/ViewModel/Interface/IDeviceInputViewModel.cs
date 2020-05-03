using MoonBurst.Api.Hardware;
using MoonBurst.Api.Parser;

namespace MoonBurst.ViewModel
{
    public interface IDeviceInputViewModel
    {
        IFootswitchState State { get; set; }
        IArduinoPort Port { get; set; }
        IDeviceDefinition Device { get; set; }
        IDeviceInput Input { get; set; }
        string PortName { get; }
        string DeviceName { get; }
        string ControllerName { get; }
        string ControllerNameWithoutHeader { get; }
        string FormatedName { get; }
    }
}
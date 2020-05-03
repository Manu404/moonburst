using MoonBurst.Api.Gateway.Arduino;
using MoonBurst.Api.Hardware;
using MoonBurst.Api.Hardware.Description;
using MoonBurst.Api.Hardware.Parser;

namespace MoonBurst.ViewModel.Interface
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
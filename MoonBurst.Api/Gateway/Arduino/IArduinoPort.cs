using System.Collections.Generic;
using MoonBurst.Api.Hardware;

namespace MoonBurst.Api.Gateway.Arduino
{
    public interface IArduinoPort
    {
        int Position { get; }
        IDeviceDefinition ConnectedDevice { get; set; }
        IList<IDeviceDefinition> AvailableDevices { get; }
    }
}
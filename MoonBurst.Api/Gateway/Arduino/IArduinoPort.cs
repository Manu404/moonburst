using System.Collections.Generic;

namespace MoonBurst.Api.Hardware
{
    public interface IArduinoPort
    {
        int Position { get; }
        IDeviceDefinition ConnectedDevice { get; set; }
        IList<IDeviceDefinition> AvailableDevices { get; }
    }
}
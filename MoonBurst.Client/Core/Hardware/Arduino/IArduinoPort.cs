using System.Collections.Generic;

namespace MoonBurst.Core
{
    public interface IArduinoPort
    {
        int Position { get; }
        IDeviceDefinition ConnectedDevice { get; set; }
        IList<IDeviceDefinition> AvailableDevices { get; }
    }
}
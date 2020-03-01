using System.Collections.Generic;

namespace MoonBurst.Core
{
    public interface IArduinoGateway
    {
        IArduinoPort[] Ports { get; }
        List<IDeviceDefinition> GetConnectedDevices();
    }
}
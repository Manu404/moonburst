using MoonBurst.Api.Hardware;
using System.Collections.Generic;

namespace MoonBurst.Api.Gateways
{
    public interface IArduinoGateway
    {
        IArduinoPort[] Ports { get; }
        List<IDeviceDefinition> GetConnectedDevices();
    }
}
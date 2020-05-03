using System.Collections.Generic;
using MoonBurst.Api.Hardware;

namespace MoonBurst.Api.Gateway.Arduino
{
    public interface IArduinoGateway
    {
        IArduinoPort[] Ports { get; }
        List<IDeviceDefinition> GetConnectedDevices();
    }
}
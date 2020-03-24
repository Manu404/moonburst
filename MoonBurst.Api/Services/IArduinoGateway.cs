using MoonBurst.Api.Hardware;
using System.Collections.Generic;

namespace MoonBurst.Api.Services
{
    public interface IArduinoGateway
    {
        IArduinoPort[] Ports { get; }
        List<IDeviceDefinition> GetConnectedDevices();
    }
}
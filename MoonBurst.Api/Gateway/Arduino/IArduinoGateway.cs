using System;
using System.Collections.Generic;
using MoonBurst.Api.Hardware.Description;

namespace MoonBurst.Api.Gateway.Arduino
{
    public interface IArduinoGateway
    {
        IArduinoPort[] Ports { get; }
        List<IDeviceDefinition> GetConnectedDevices();
        event EventHandler ConnectedDevicesChanged;
    }
}
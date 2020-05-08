using System;
using System.Collections.Generic;
using MoonBurst.Api.Hardware.Description;

namespace MoonBurst.Api.Gateway.Arduino
{
    public interface IArduinoPort
    {
        int Position { get; }
        IDeviceDefinition ConnectedDevice { get; set; }
        IList<IDeviceDefinition> AvailableDevices { get; }
        event EventHandler ConnectedDeviceChanged;
    }
}
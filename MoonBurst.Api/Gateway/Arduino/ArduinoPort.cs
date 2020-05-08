using System;
using System.Collections.Generic;
using MoonBurst.Api.Hardware.Description;

namespace MoonBurst.Api.Gateway.Arduino
{
    public class ArduinoPort : IArduinoPort
    {
        private IDeviceDefinition _connectedDevice;
        public int Position { get; }

        public IDeviceDefinition ConnectedDevice
        {
            get => _connectedDevice;
            set
            {
                _connectedDevice = value;
                ConnectedDeviceChanged?.Invoke(this, new EventArgs());
            }
        }

        public IList<IDeviceDefinition> AvailableDevices { get; }
        public event EventHandler ConnectedDeviceChanged;

        public ArduinoPort(int index, IDeviceDefinition[] deviceDefinitions)
        {
            Position = index;
            AvailableDevices = new List<IDeviceDefinition>(deviceDefinitions);
        }
    }
}
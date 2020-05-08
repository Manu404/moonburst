using System;
using System.Collections.Generic;
using System.Linq;
using MoonBurst.Api.Gateway;
using MoonBurst.Api.Gateway.Arduino;
using MoonBurst.Api.Hardware.Description;

namespace MoonBurst.Core.Gateway
{

    public class ArduinoGateway : IArduinoGateway, IGateway
    {
        public IArduinoPort[] Ports { get; }
        public IDeviceDefinition[] DeviceDefinitions { get; }

        public List<IDeviceDefinition> GetConnectedDevices()
        {
            return Ports.Select(p => p.ConnectedDevice).ToList();
        }

        public event EventHandler ConnectedDevicesChanged;

        public ArduinoGateway(IDeviceDefinition[] deviceDefinitions)
        {
            int portCount = 6;
            Ports = new IArduinoPort[portCount];
            DeviceDefinitions = deviceDefinitions;
            InitializePorts();
        }

        private void InitializePorts()
        {
            for (int i = 0; i < Ports.Length; i++)
            {
                Ports[i] = new ArduinoPort(i, DeviceDefinitions);
                Ports[i].ConnectedDeviceChanged += OnConnectedDeviceChanged;
            }
        }

        private void OnConnectedDeviceChanged(object sender, EventArgs e)
        {
            // forward event from ports
            ConnectedDevicesChanged?.Invoke(this, e);
        }
    }
}
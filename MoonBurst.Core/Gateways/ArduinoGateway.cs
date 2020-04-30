using System.Collections.Generic;
using System.Linq;
using MoonBurst.Api.Gateways;
using MoonBurst.Api.Hardware;
using MoonBurst.Core.Arduino;

namespace MoonBurst.Core.Gateways
{

    public class ArduinoGateway : IArduinoGateway, IGateway
    {
        public IArduinoPort[] Ports { get; }
        public IDeviceDefinition[] DeviceDefinitions { get; }

        public List<IDeviceDefinition> GetConnectedDevices()
        {
            return Ports.Select(p => p.ConnectedDevice).ToList();
        } 

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
            }
        }
    }
}
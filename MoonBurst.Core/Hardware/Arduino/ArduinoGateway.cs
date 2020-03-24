using System.Collections.Generic;
using System.Linq;
using MoonBurst.Api;
using MoonBurst.Api.Hardware;
using MoonBurst.Api.Services;

namespace MoonBurst.Core
{

    public class ArduinoGateway : IArduinoGateway, IHardwareService
    {
        public IArduinoPort[] Ports { get; }

        public List<IDeviceDefinition> GetConnectedDevices()
        {
            return Ports.Select(p => p.ConnectedDevice).ToList();
        } 

        public ArduinoGateway()
        {
            int portCount = 6;
            Ports = new IArduinoPort[portCount];
            InitializePorts();
        }

        private void InitializePorts()
        {
            for (int i = 0; i < Ports.Length; i++)
            {
                Ports[i] = new ArduinoPort(i);
            }
        }
    }
}
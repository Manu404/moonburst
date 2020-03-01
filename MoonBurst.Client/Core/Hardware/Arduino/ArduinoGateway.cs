using System.Collections.Generic;
using System.Linq;
using MoonBurst.Model;

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
            int portCount = 4;
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
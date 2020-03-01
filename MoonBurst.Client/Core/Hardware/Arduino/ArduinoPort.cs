using System.Collections.Generic;

namespace MoonBurst.Core
{
    public class ArduinoPort : IArduinoPort
    {
        public int Position { get; }
        public IDeviceDefinition ConnectedDevice { get; set; }
        public IList<IDeviceDefinition> AvailableDevices { get; }

        public ArduinoPort(int index)
        {
            Position = index;
            AvailableDevices = new List<IDeviceDefinition>();
            AvailableDevices.Add(new Fs3xDeviceDefinition());
        }
    }
}
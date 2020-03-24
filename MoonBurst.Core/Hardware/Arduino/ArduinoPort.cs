using System.Collections.Generic;
using MoonBurst.Api.Hardware;

namespace MoonBurst.Core.Hardware.Arduino
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
            AvailableDevices.Add(new Fs3XDeviceDefinition());
        }
    }
}
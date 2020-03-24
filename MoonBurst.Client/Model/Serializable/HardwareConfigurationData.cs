using System.Collections.Generic;
using MoonBurst.Api.Services;

namespace MoonBurst.Model.Serializable
{
    public class HardwareConfigurationData
    {
        public InputComPortData ComPort { get; set; }
        public OutputMidiDeviceData MidiOut { get; set; }
        public List<ArduinoPortConfigData> ArduinoPorts { get; set; }
        public int Speed { get; set; }
    }
}
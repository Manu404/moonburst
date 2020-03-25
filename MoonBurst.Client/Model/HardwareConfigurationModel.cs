using System.Collections.Generic;
using MoonBurst.Api.Services;

namespace MoonBurst.Model
{
    public class HardwareConfigurationModel
    {
        public InputComPortData ComPort { get; set; }
        public OutputMidiDeviceData MidiOut { get; set; }
        public List<ArduinoPortConfigModel> ArduinoPorts { get; set; }
        public int Speed { get; set; }
    }
}
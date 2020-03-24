using System.Collections.Generic;
using MoonBurst.Api.Services;
using MoonBurst.Core;
using MoonBurst.Model;

namespace MoonBurst.ViewModel
{
    public class HardwareConfigurationData
    {
        public InputCOMPortData ComPort { get; set; }
        public OutputMidiDeviceData MidiOut { get; set; }
        public List<ArduinoPortConfigData> ArduinoPorts { get; set; }
        public int Speed { get; set; }
    }
}
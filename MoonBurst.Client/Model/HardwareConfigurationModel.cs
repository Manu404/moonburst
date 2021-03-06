﻿using System.Collections.Generic;
using MoonBurst.Api.Gateway.Midi;
using MoonBurst.Api.Gateway.Serial;

namespace MoonBurst.Model
{
    public class HardwareConfigurationModel
    {
        public ComPort ComPort { get; set; }
        public MidiDevice MidiOut { get; set; }
        public List<ArduinoPortConfigModel> ArduinoPorts { get; set; }
        public ComPortSpeed Speed { get; set; }

        public HardwareConfigurationModel()
        {
            ArduinoPorts = new List<ArduinoPortConfigModel>();
        }
    }
}
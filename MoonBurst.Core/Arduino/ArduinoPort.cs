﻿using System.Collections.Generic;
using MoonBurst.Api.Hardware;

namespace MoonBurst.Core.Arduino
{
    public class ArduinoPort : IArduinoPort
    {
        public int Position { get; }
        public IDeviceDefinition ConnectedDevice { get; set; }
        public IList<IDeviceDefinition> AvailableDevices { get; }

        public ArduinoPort(int index, IDeviceDefinition[] deviceDefinitions)
        {
            Position = index;
            AvailableDevices = new List<IDeviceDefinition>(deviceDefinitions);
        }
    }
}
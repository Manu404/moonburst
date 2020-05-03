using System;
using MoonBurst.Api.Hardware.Parser;

namespace MoonBurst.Api.Gateway.Serial
{
    public class ControllerStateEventArgs : EventArgs
    {
        public ControllerStateEventArgs(IDeviceInputState[] states, int port)
        {
            States = states;
            Port = port;
        }

        public IDeviceInputState[] States { get; }
        public int Port { get; }
    }
}
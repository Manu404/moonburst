using System;
using MoonBurst.Api.Hardware.Parser;

namespace MoonBurst.Api.Gateway.Serial
{
    public class ControllerStateEventArgs : EventArgs
    {
        public ControllerStateEventArgs(FootswitchState[] states, int port)
        {
            States = states;
            Port = port;
        }

        public FootswitchState[] States { get; }
        public int Port { get; }
    }
}
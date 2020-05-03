using System;
using MoonBurst.Api.Parser;

namespace MoonBurst.Api.Gateways
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
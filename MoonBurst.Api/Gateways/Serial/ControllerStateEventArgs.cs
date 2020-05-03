using System;
using MoonBurst.Api.Parser;

namespace MoonBurst.Api.Gateways
{
    public class ControllerStateEventArgs : EventArgs
    {
        public ControllerStateEventArgs(MomentaryFootswitchState[] states, int port)
        {
            States = states;
            Port = port;
        }

        public MomentaryFootswitchState[] States { get; }
        public int Port { get; }
    }
}
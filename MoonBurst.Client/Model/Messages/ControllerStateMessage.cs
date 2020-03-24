using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Parser;

namespace MoonBurst.Model.Messages
{
    public class ControllerStateMessage : MessageBase
    {
        public ControllerStateMessage(MomentaryFootswitchState[] states, int port)
        {
            States = states;
            Port = port;
        }

        public MomentaryFootswitchState[] States { get; }
        public int Port { get; }
    }
}
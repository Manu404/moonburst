using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Parser;

namespace MoonBurst.Model.Messages
{
    public class ControllerStateMessage : MessageBase
    {
        public MomentaryFootswitchState[] States { get; set; }
        public int Port { get; set; }
    }
}
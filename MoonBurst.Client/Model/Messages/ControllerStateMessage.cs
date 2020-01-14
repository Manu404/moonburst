using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Model.Parser;

namespace MoonBurst.Model.Messages
{
    public class ControllerStateMessage : MessageBase
    {
        public List<MomentaryFootswitchState> States { get; set; }
        public int Port { get; set; }
    }
}
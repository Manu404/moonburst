using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Enums;

namespace MoonBurst.Model.Messages
{
    public class MidiConnectionStateChangedMessage : MessageBase
    {
        public MidiConnectionStatus NewState { get; set; }
        public MidiConnectionStatus PreviousState { get; set; }
    }
}
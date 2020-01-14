using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Model;

namespace MoonBurst.Model.Messages
{
    public class MidiConnectionStateChangedMessage : MessageBase
    {
        public MidiConnectionStatus NewState { get; set; }
        public MidiConnectionStatus PreviousState { get; set; }
    }
}
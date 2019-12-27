using GalaSoft.MvvmLight.Messaging;

namespace MoonBurst.Core
{
    public class MidiConnectionStateChangedMessage : MessageBase
    {
        public MidiConnectionStatus NewState { get; set; }
        public MidiConnectionStatus PreviousState { get; set; }
    }
}
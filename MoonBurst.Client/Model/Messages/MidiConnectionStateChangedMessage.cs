using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Enums;

namespace MoonBurst.Model.Messages
{
    public class MidiConnectionStateChangedMessage : MessageBase
    {
        public MidiConnectionStateChangedMessage(MidiConnectionStatus newState, MidiConnectionStatus previousState)
        {
            NewState = newState;
            PreviousState = previousState;
        }

        public MidiConnectionStatus NewState { get; }
        public MidiConnectionStatus PreviousState { get; }
    }
}
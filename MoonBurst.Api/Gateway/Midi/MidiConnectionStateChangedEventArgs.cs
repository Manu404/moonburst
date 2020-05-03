using System;
using MoonBurst.Api.Enums;

namespace MoonBurst.Api.Gateway.Midi
{
    public class MidiConnectionStateChangedEventArgs : EventArgs
    {          
        public MidiConnectionStateChangedEventArgs(MidiConnectionState newState, MidiConnectionState previousState)
        {
            NewState = newState;
            PreviousState = previousState;
        }

        public MidiConnectionState NewState { get; }
        public MidiConnectionState PreviousState { get; }
    }
}
using System;
using MoonBurst.Api.Enums;

namespace MoonBurst.Api.Gateways
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
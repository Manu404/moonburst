using System;

namespace MoonBurst.Api.Gateways
{
    public class SerialConnectionStateChangedEventArgs : EventArgs
    {
        public SerialConnectionStateChangedEventArgs(bool newState)
        {
            NewState = newState;
        }

        public bool NewState { get; }
    }
}
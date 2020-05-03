using System;

namespace MoonBurst.Api.Gateway.Serial
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
using GalaSoft.MvvmLight.Messaging;

namespace MoonBurst.Model.Messages
{
    public class SerialConnectionStateChangedMessage : MessageBase
    {
        public SerialConnectionStateChangedMessage(bool newState)
        {
            NewState = newState;
        }

        public bool NewState { get; }
    }
}
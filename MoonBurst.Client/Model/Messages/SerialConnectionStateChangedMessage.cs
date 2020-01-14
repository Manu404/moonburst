using GalaSoft.MvvmLight.Messaging;

namespace MoonBurst.Model.Messages
{
    public class SerialConnectionStateChangedMessage : MessageBase
    {
        public bool NewState { get; set; }
    }
}
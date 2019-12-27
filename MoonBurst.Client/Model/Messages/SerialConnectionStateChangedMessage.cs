using GalaSoft.MvvmLight.Messaging;

namespace MoonBurst.ViewModel
{
    public class SerialConnectionStateChangedMessage : MessageBase
    {
        public bool NewState { get; set; }
    }
}
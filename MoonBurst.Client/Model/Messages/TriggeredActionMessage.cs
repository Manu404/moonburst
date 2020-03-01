using GalaSoft.MvvmLight.Messaging;
using MoonBurst.ViewModel;

namespace MoonBurst.Model.Messages
{
    public class TriggeredActionMessage : MessageBase
    {
        public IFunctoidActionViewModel Data { get; set; }
    }
}
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.ViewModel;

namespace MoonBurst.Model.Messages
{
    public class TriggeredActionMessage : MessageBase
    {
        public FunctoidActionViewModel.FunctoidActionData Data { get; set; }
    }
}
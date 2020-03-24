using GalaSoft.MvvmLight.Messaging;
using MoonBurst.ViewModel;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.Model.Messages
{
    public class TriggeredActionMessage : MessageBase
    {
        public IFunctoidActionViewModel Data { get; set; }
    }
}
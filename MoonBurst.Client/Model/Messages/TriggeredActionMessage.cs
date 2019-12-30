using GalaSoft.MvvmLight.Messaging;

namespace MoonBurst.ViewModel
{
    public class TriggeredActionMessage : MessageBase
    {
        public FunctoidActionViewModel.FunctoidActionData Data { get; set; }
    }
}
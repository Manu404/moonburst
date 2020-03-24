using GalaSoft.MvvmLight.Messaging;
using MoonBurst.ViewModel;

namespace MoonBurst.Model.Messages
{
    public class DeleteFunctoidActionMessage : MessageBase
    {
        public FunctoidActionViewModel Item { get; }

        public DeleteFunctoidActionMessage(FunctoidActionViewModel item)
        {
            Item = item;
        }
    }
}
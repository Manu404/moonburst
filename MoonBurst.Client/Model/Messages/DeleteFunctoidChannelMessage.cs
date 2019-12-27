using GalaSoft.MvvmLight.Messaging;
using MoonBurst.ViewModel;

namespace MoonBurst.Model
{
    public class DeleteFunctoidChannelMessage : MessageBase
    {
        public FunctoidChannelViewModel Item { get; }

        public DeleteFunctoidChannelMessage(FunctoidChannelViewModel item)
        {
            Item = item;
        }
    }

    public class DeleteFunctoidActionMessage : MessageBase
    {
        public FunctoidActionViewModel Item { get; }

        public DeleteFunctoidActionMessage(FunctoidActionViewModel item)
        {
            Item = item;
        }
    }
}
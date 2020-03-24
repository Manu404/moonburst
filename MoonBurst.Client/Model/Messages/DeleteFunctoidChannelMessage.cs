using GalaSoft.MvvmLight.Messaging;
using MoonBurst.ViewModel;

namespace MoonBurst.Model.Messages
{
    public class DeleteFunctoidChannelMessage : MessageBase
    {
        public FunctoidChannelViewModel Item { get; }

        public DeleteFunctoidChannelMessage(FunctoidChannelViewModel item)
        {
            Item = item;
        }
    }
}
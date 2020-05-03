using GalaSoft.MvvmLight.Messaging;
using MoonBurst.ViewModel;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.Model.Messages
{
    public class DeleteChannelActionMessage : MessageBase
    {
        public IChannelActionViewModel Item { get; }

        public DeleteChannelActionMessage(IChannelActionViewModel item)
        {
            Item = item;
        }
    }
}
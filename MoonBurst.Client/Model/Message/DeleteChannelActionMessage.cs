using GalaSoft.MvvmLight.Messaging;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.Model.Message
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
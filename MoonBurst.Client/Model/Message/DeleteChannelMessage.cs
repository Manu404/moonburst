using GalaSoft.MvvmLight.Messaging;
using MoonBurst.ViewModel;

namespace MoonBurst.Model.Message
{
    public class DeleteChannelMessage : MessageBase
    {
        public LayoutChannelViewModel Item { get; }

        public DeleteChannelMessage(LayoutChannelViewModel item)
        {
            Item = item;
        }
    }
}
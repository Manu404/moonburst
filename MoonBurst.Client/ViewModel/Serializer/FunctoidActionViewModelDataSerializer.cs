using MoonBurst.Api.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel.Serializer
{
    public class FunctoidActionViewModelDataSerializer : IDataExtractor<IChannelActionViewModel, ChannelActionModel>
    {
        public ChannelActionModel ExtractData(IChannelActionViewModel source)
        {
            return new ChannelActionModel()
            {
                MidiChannel = source.MidiChannel,
                Data1 = source.Data1,
                Data2 = source.Data2,
                Command = source.Command,
                Trigger = source.Trigger,
                IsEnabled = source.IsEnabled,
                IsExpanded = source.IsExpanded,
                IsLocked = source.IsLocked
            };
        }

        public void ApplyData(ChannelActionModel model, IChannelActionViewModel target)
        {
            target.MidiChannel = model.MidiChannel;
            target.Data2 = model.Data2;
            target.Data1 = model.Data1;
            target.Command = model.Command;
            target.Trigger = model.Trigger;
            target.IsEnabled = model.IsEnabled;
            target.IsExpanded = model.IsExpanded;
            target.IsLocked = model.IsLocked;

        }
    }
}
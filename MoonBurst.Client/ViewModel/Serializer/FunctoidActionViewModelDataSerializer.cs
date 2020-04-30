using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Serializer
{
    public class FunctoidActionViewModelDataSerializer : IDataExtractor<IFunctoidActionViewModel, FunctoidActionModel>
    {
        public FunctoidActionModel ExtractData(IFunctoidActionViewModel source)
        {
            return new FunctoidActionModel()
            {
                MidiChannel = source.MidiChannel,
                Data1 = source.Data1,
                Data2 = source.Data2,
                Command = source.Command,
                Trigger = source.Trigger,
                IsEnabled = source.IsEnabled,
                IsExpanded = source.IsExpanded
            };
        }

        public void ApplyData(FunctoidActionModel model, IFunctoidActionViewModel target)
        {
            target.MidiChannel = model.MidiChannel;
            target.Data2 = model.Data2;
            target.Data1 = model.Data1;
            target.Command = model.Command;
            target.Trigger = model.Trigger;
            target.IsEnabled = model.IsEnabled;
            target.IsExpanded = model.IsExpanded;
        }
    }
}
using MoonBurst.Core;

namespace MoonBurst.ViewModel
{
    public class FunctoidActionViewModelDataSerializer : IDataExtractor<IFunctoidActionViewModel, FunctoidActionData>
    {
        public FunctoidActionData ExtractData(IFunctoidActionViewModel source)
        {
            return new FunctoidActionData()
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

        public void ApplyData(FunctoidActionData data, IFunctoidActionViewModel target)
        {
            target.MidiChannel = data.MidiChannel;
            target.Data2 = data.Data2;
            target.Data1 = data.Data1;
            target.Command = data.Command;
            target.Trigger = data.Trigger;
            target.IsEnabled = data.IsEnabled;
            target.IsExpanded = data.IsExpanded;
        }
    }
}
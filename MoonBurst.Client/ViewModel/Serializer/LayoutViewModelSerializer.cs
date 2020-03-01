using System.Linq;
using MoonBurst.Core;

namespace MoonBurst.ViewModel
{
    public class LayoutViewModelSerializer : SerializerBase<ILayoutViewModel, LayoutData>
    {
        IFunctoidChannelViewModelFactory _channelFactory;
        IExtractor<IFunctoidChannelViewModel, FunctoidChannelData> _extractor;

        public override string Default { get => "default_layout.xml"; }

        public LayoutViewModelSerializer(IFunctoidChannelViewModelFactory factory, IExtractor<IFunctoidChannelViewModel, FunctoidChannelData> extractor)
        {
            _channelFactory = factory;
            _extractor = extractor;
        }

        public override LayoutData ExtractData(ILayoutViewModel source)
        {
            return new LayoutData()
            {
                Channels = source.FunctoidChannels.ToList().ConvertAll(f => _extractor.ExtractData(f))
            };
        }

        public override void ApplyData(LayoutData config, ILayoutViewModel target)
        {
            target.FunctoidChannels.Clear();
            config.Channels.ConvertAll(data => _channelFactory.Build(data)).ForEach(target.FunctoidChannels.Add);
            target.FunctoidChannels.ToList().ForEach(d => d.RefreshInputs());
        }
    }
}
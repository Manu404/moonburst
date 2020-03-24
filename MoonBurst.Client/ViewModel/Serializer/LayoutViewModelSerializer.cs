using System.Linq;
using MoonBurst.Core.Serializer;
using MoonBurst.Model.Serializable;
using MoonBurst.ViewModel.Factories;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Serializer
{
    public class LayoutViewModelSerializer : SerializerBase<ILayoutViewModel, LayoutData>
    {
        IFunctoidChannelViewModelFactory _channelFactory;
        IDataExtractor<IFunctoidChannelViewModel, FunctoidChannelData> _extractor;

        public LayoutViewModelSerializer(IFunctoidChannelViewModelFactory factory, IDataExtractor<IFunctoidChannelViewModel, FunctoidChannelData> extractor) : base("default_layout.xml")
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
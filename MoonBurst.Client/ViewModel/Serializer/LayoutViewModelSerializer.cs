using System.Linq;
using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Factories;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Serializer
{
    public class LayoutViewModelSerializer : SerializerBase<ILayoutViewModel, LayoutModel>
    {
        IFunctoidChannelViewModelFactory _channelFactory;
        IDataExtractor<IFunctoidChannelViewModel, FunctoidChannelModel> _extractor;

        public LayoutViewModelSerializer(IFunctoidChannelViewModelFactory factory, IDataExtractor<IFunctoidChannelViewModel, FunctoidChannelModel> extractor) : base("default_layout.xml")
        {
            _channelFactory = factory;
            _extractor = extractor;
        }

        public override LayoutModel ExtractData(ILayoutViewModel source)
        {
            return new LayoutModel()
            {
                Channels = source.FunctoidChannels.ToList().ConvertAll(f => _extractor.ExtractData(f))
            };
        }

        public override void ApplyData(LayoutModel config, ILayoutViewModel target)
        {
            target.FunctoidChannels.Clear();
            config.Channels.ConvertAll(data => _channelFactory.Build(data)).ForEach(target.FunctoidChannels.Add);
            target.FunctoidChannels.ToList().ForEach(d => d.RefreshInputs());
        }
    }
}
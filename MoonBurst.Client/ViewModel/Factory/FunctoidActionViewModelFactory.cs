using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Client;
using MoonBurst.Api.Gateway.Midi;
using MoonBurst.Api.Helper;
using MoonBurst.Api.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel.Factory
{
    public interface IChannelActionViewModelFactory : IFactory<IChannelActionViewModel>, IFactory<IChannelActionViewModel, ChannelActionModel>
    {

    }

    public class ChannelActionViewModelFactory : IChannelActionViewModelFactory
    {
        private readonly INoteHelper _noteHelper;
        private readonly IDynamicsHelper _dynamicsHelper;
        private readonly IMidiGateway _midiGateway;
        private readonly IDataExtractor<IChannelActionViewModel, ChannelActionModel> _actionExtractor;

        public ChannelActionViewModelFactory(
            INoteHelper noteHelper,
            IDynamicsHelper dynamicsHelper,
            IDataExtractor<IChannelActionViewModel, ChannelActionModel> actionExtractor,
            IMidiGateway midiGateway)
        {
            _noteHelper = noteHelper;
            _dynamicsHelper = dynamicsHelper;
            _actionExtractor = actionExtractor;
            _midiGateway = midiGateway;
        }

        public IChannelActionViewModel Build(ChannelActionModel model)
        {
            var vm = Build();
            _actionExtractor.ApplyData(model, vm);
            return vm;
        }

        public IChannelActionViewModel Build()
        {
            return new ChannelActionViewModel(_noteHelper, _dynamicsHelper, _midiGateway);
        }
    }
}
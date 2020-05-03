using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Gateways;
using MoonBurst.Core;
using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Factories
{
    public interface IFunctoidChannelViewModelFactory : IFactory<ILayoutChannelViewModel>, IFactory<ILayoutChannelViewModel, LayoutChannelModel>
    {

    }

    public class FunctoidChannelViewModelFactory : IFunctoidChannelViewModelFactory
    {
        private readonly IArduinoGateway _arduinoGateway;
        private readonly ISerialGateway _serialGateway;
        private readonly IMessenger _messenger;
        private readonly IChannelActionViewModelFactory _factory;
        private readonly IDataExtractor<ILayoutChannelViewModel, LayoutChannelModel> _channelExtractor;
        private readonly IFactory<IDeviceInputViewModel> _deviceInputViewModelFactory;

        public FunctoidChannelViewModelFactory(IArduinoGateway arduinoGateway, 
            ISerialGateway serialGateway,
            IMessenger messenger, 
            IChannelActionViewModelFactory factory,
            IDataExtractor<ILayoutChannelViewModel, LayoutChannelModel> extractor,
            IFactory<IDeviceInputViewModel> deviceInputViewModelFactory)
        {
            _arduinoGateway = arduinoGateway;
            _messenger = messenger;
            _channelExtractor = extractor;
            _factory = factory;
            _deviceInputViewModelFactory = deviceInputViewModelFactory;
            _serialGateway = serialGateway;
        }

        public ILayoutChannelViewModel Build(LayoutChannelModel model)
        {
            var vm = Build();
            _channelExtractor.ApplyData(model, vm);
            return vm;
        }

        public ILayoutChannelViewModel Build()
        {
            return new LayoutChannelViewModel(_messenger, _arduinoGateway, _factory, _deviceInputViewModelFactory, _serialGateway);
        }
    }
}
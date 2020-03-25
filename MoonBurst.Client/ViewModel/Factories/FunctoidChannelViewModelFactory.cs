using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Services;
using MoonBurst.Core;
using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Factories
{
    public interface IFunctoidChannelViewModelFactory : IFactory<IFunctoidChannelViewModel>, IFactory<IFunctoidChannelViewModel, FunctoidChannelModel>
    {

    }

    public class FunctoidChannelViewModelFactory : IFunctoidChannelViewModelFactory
    {
        private readonly IArduinoGateway _arduinoGateway;
        private readonly ISerialGateway _serialGateway;
        private readonly IMessenger _messenger;
        private readonly IFunctoidActionViewModelFactory _factory;
        private readonly IDataExtractor<IFunctoidChannelViewModel, FunctoidChannelModel> _channelExtractor;
        private readonly IFactory<IDeviceInputViewModel> _deviceInputViewModelFactory;

        public FunctoidChannelViewModelFactory(IArduinoGateway arduinoGateway, 
            ISerialGateway serialGateway,
            IMessenger messenger, 
            IFunctoidActionViewModelFactory factory,
            IDataExtractor<IFunctoidChannelViewModel, FunctoidChannelModel> extractor,
            IFactory<IDeviceInputViewModel> deviceInputViewModelFactory)
        {
            _arduinoGateway = arduinoGateway;
            _messenger = messenger;
            _channelExtractor = extractor;
            _factory = factory;
            _deviceInputViewModelFactory = deviceInputViewModelFactory;
            _serialGateway = serialGateway;
        }

        public IFunctoidChannelViewModel Build(FunctoidChannelModel model)
        {
            var vm = Build();
            _channelExtractor.ApplyData(model, vm);
            return vm;
        }

        public IFunctoidChannelViewModel Build()
        {
            return new FunctoidChannelViewModel(_messenger, _arduinoGateway, _factory, _deviceInputViewModelFactory, _serialGateway);
        }
    }
}
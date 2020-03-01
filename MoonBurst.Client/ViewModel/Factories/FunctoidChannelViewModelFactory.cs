using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Core;

namespace MoonBurst.ViewModel
{
    public interface IFunctoidChannelViewModelFactory : IFactory<IFunctoidChannelViewModel>, IFactory<IFunctoidChannelViewModel, FunctoidChannelData>
    {

    }

    public class FunctoidChannelViewModelFactory : IFunctoidChannelViewModelFactory
    {
        IArduinoGateway _arduinoGateway;
        IMessenger _messenger;
        IFunctoidActionViewModelFactory _factory;
        IExtractor<IFunctoidChannelViewModel, FunctoidChannelData> _channel_extractor;

        public FunctoidChannelViewModelFactory(IArduinoGateway arduinoGateway, 
            IMessenger messenger, 
            IFunctoidActionViewModelFactory factory,
            IExtractor<IFunctoidChannelViewModel, FunctoidChannelData> extractor)
        {
            _arduinoGateway = arduinoGateway;
            _messenger = messenger;
            _channel_extractor = extractor;
            _factory = factory;
        }

        public IFunctoidChannelViewModel Build(FunctoidChannelData data)
        {
            var vm = Build();
            _channel_extractor.ApplyData(data, vm);
            return vm;
        }

        public IFunctoidChannelViewModel Build()
        {
            return new FunctoidChannelViewModel(_messenger, _arduinoGateway, _factory);
        }
    }
}
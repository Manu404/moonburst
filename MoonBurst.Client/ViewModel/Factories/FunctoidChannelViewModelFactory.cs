using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Services;
using MoonBurst.Core;
using MoonBurst.Core.Serializer;
using MoonBurst.Model.Serializable;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Factories
{
    public interface IFunctoidChannelViewModelFactory : IFactory<IFunctoidChannelViewModel>, IFactory<IFunctoidChannelViewModel, FunctoidChannelData>
    {

    }

    public class FunctoidChannelViewModelFactory : IFunctoidChannelViewModelFactory
    {
        IArduinoGateway _arduinoGateway;
        IMessenger _messenger;
        IFunctoidActionViewModelFactory _factory;
        IDataExtractor<IFunctoidChannelViewModel, FunctoidChannelData> _channel_extractor;

        public FunctoidChannelViewModelFactory(IArduinoGateway arduinoGateway, 
            IMessenger messenger, 
            IFunctoidActionViewModelFactory factory,
            IDataExtractor<IFunctoidChannelViewModel, FunctoidChannelData> extractor)
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
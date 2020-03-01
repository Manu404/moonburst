using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Core;
using MoonBurst.Core.Helper;

namespace MoonBurst.ViewModel
{
    public interface IFunctoidActionViewModelFactory : IFactory<IFunctoidActionViewModel>, IFactory<IFunctoidActionViewModel, FunctoidActionData>
    {

    }

    public class FunctoidActionViewModelFactory : IFunctoidActionViewModelFactory
    {
        private IArduinoGateway _arduinoGateway;
        private IMessenger _messenger;
        private IMusicalNoteHelper _noteHelper;
        private IExtractor<IFunctoidActionViewModel, FunctoidActionData> _actionExtractor;

        public FunctoidActionViewModelFactory(IMessenger messenger,             
            IArduinoGateway arduinoGateway, 
            IMusicalNoteHelper noteHelper,
            IExtractor<IFunctoidActionViewModel, FunctoidActionData> actionExtractor)
        {
            _arduinoGateway = arduinoGateway;
            _messenger = messenger;
            _noteHelper = noteHelper;
            _actionExtractor = actionExtractor;
        }

        public IFunctoidActionViewModel Build(FunctoidActionData data)
        {
            var vm = Build();
            _actionExtractor.ApplyData(data, vm);
            return vm;
        }

        public IFunctoidActionViewModel Build()
        {
            return new FunctoidActionViewModel(_messenger, _arduinoGateway, _noteHelper);
        }
    }
}
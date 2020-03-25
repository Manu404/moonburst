using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Gateways;
using MoonBurst.Core;
using MoonBurst.Core.Helper;
using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Factories
{
    public interface IFunctoidActionViewModelFactory : IFactory<IFunctoidActionViewModel>, IFactory<IFunctoidActionViewModel, FunctoidActionModel>
    {

    }

    public class FunctoidActionViewModelFactory : IFunctoidActionViewModelFactory
    {
        private IMessenger _messenger;
        private IMusicalNoteHelper _noteHelper;
        private IDynamicsHelper _dynamicsHelper;
        private IMidiGateway _midiGateway;
        private IDataExtractor<IFunctoidActionViewModel, FunctoidActionModel> _actionExtractor;

        public FunctoidActionViewModelFactory(
            IMessenger messenger,
            IMusicalNoteHelper noteHelper,
            IDynamicsHelper dynamicsHelper,
            IDataExtractor<IFunctoidActionViewModel, FunctoidActionModel> actionExtractor,
            IMidiGateway midiGateway)
        {
            _messenger = messenger;
            _noteHelper = noteHelper;
            _dynamicsHelper = dynamicsHelper;
            _actionExtractor = actionExtractor;
            _midiGateway = midiGateway;
        }

        public IFunctoidActionViewModel Build(FunctoidActionModel model)
        {
            var vm = Build();
            _actionExtractor.ApplyData(model, vm);
            return vm;
        }

        public IFunctoidActionViewModel Build()
        {
            return new FunctoidActionViewModel(_messenger, _noteHelper, _dynamicsHelper, _midiGateway);
        }
    }
}
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Services;
using MoonBurst.Core;
using MoonBurst.Core.Helper;
using MoonBurst.Core.Serializer;
using MoonBurst.Model.Serializable;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Factories
{
    public interface IFunctoidActionViewModelFactory : IFactory<IFunctoidActionViewModel>, IFactory<IFunctoidActionViewModel, FunctoidActionData>
    {

    }

    public class FunctoidActionViewModelFactory : IFunctoidActionViewModelFactory
    {
        private IMessenger _messenger;
        private IMusicalNoteHelper _noteHelper;
        private IDynamicsHelper _dynamicsHelper;
        private IMidiGateway _midiGateway;
        private IDataExtractor<IFunctoidActionViewModel, FunctoidActionData> _actionExtractor;

        public FunctoidActionViewModelFactory(
            IMessenger messenger,
            IMusicalNoteHelper noteHelper,
            IDynamicsHelper dynamicsHelper,
            IDataExtractor<IFunctoidActionViewModel, FunctoidActionData> actionExtractor,
            IMidiGateway midiGateway)
        {
            _messenger = messenger;
            _noteHelper = noteHelper;
            _dynamicsHelper = dynamicsHelper;
            _actionExtractor = actionExtractor;
            _midiGateway = midiGateway;
        }

        public IFunctoidActionViewModel Build(FunctoidActionData data)
        {
            var vm = Build();
            _actionExtractor.ApplyData(data, vm);
            return vm;
        }

        public IFunctoidActionViewModel Build()
        {
            return new FunctoidActionViewModel(_messenger, _noteHelper, _dynamicsHelper, _midiGateway);
        }
    }
}
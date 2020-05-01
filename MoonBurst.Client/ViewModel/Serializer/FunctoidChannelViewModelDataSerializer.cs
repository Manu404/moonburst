using System.Collections.ObjectModel;
using System.Linq;
using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Factories;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel.Serializer
{
    public class FunctoidChannelViewModelDataSerializer : IDataExtractor<IFunctoidChannelViewModel, FunctoidChannelModel>
    {
        IFunctoidActionViewModelFactory _actionFactory;
        IDataExtractor<IFunctoidActionViewModel, FunctoidActionModel> _actionExtractor;

        public FunctoidChannelViewModelDataSerializer(IFunctoidActionViewModelFactory actionFactory, IDataExtractor<IFunctoidActionViewModel, FunctoidActionModel> actionExtractor)
        {
            _actionFactory = actionFactory;
            _actionExtractor = actionExtractor;
        }

        public FunctoidChannelModel ExtractData(IFunctoidChannelViewModel source)
        {
            return new FunctoidChannelModel()
            {
                Index = source.Index,
                Name = source.Name,
                Actions = source.Actions?.ToList().ConvertAll(input => _actionExtractor.ExtractData(input)),
                IsEnabled = source.IsEnabled,
                IsExpanded = source.IsExpanded,
                BindedInput = source.SelectedInput?.FormatedName,
                IsLocked = source.IsLocked
            };
        }

        public void ApplyData(FunctoidChannelModel model, IFunctoidChannelViewModel target)
        {
            target.Index = model.Index;
            target.Name = model.Name;
            target.Actions = new ObservableCollection<IFunctoidActionViewModel>(model.Actions.ConvertAll(d => _actionFactory.Build(d)));
            target.IsEnabled = model.IsEnabled;
            target.IsExpanded = model.IsExpanded;
            target.RefreshInputs();
            target.TryBindInput(model.BindedInput);
            target.IsLocked = model.IsLocked;
        }
    }
}
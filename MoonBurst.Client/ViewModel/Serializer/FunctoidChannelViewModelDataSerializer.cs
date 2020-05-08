using System.Collections.ObjectModel;
using System.Linq;
using MoonBurst.Api.Serializer;
using MoonBurst.Model;
using MoonBurst.ViewModel.Factory;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel.Serializer
{
    public class FunctoidChannelViewModelDataSerializer : IDataExtractor<ILayoutChannelViewModel, LayoutChannelModel>
    {
        readonly IChannelActionViewModelFactory _actionFactory;
        readonly IDataExtractor<IChannelActionViewModel, ChannelActionModel> _actionExtractor;

        public FunctoidChannelViewModelDataSerializer(IChannelActionViewModelFactory actionFactory, IDataExtractor<IChannelActionViewModel, ChannelActionModel> actionExtractor)
        {
            _actionFactory = actionFactory;
            _actionExtractor = actionExtractor;
        }

        public LayoutChannelModel ExtractData(ILayoutChannelViewModel source)
        {
            return new LayoutChannelModel()
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

        public void ApplyData(LayoutChannelModel model, ILayoutChannelViewModel target)
        {
            target.Index = model.Index;
            target.Name = model.Name;
            target.Actions = new ObservableCollection<IChannelActionViewModel>(model.Actions.ConvertAll(d => _actionFactory.Build(d)));
            target.IsEnabled = model.IsEnabled;
            target.IsExpanded = model.IsExpanded;
            target.RefreshInputs();
            target.TryBindInput(model.BindedInput);
            target.IsLocked = model.IsLocked;
            foreach (var a in target.Actions)
                a.IsParentChannelLocked = model.IsLocked;
        }
    }
}
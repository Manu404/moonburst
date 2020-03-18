﻿using System.Collections.ObjectModel;
using System.Linq;
using MoonBurst.Core;

namespace MoonBurst.ViewModel
{

    public class FunctoidChannelViewModelDataSerializer : IExtractor<IFunctoidChannelViewModel, FunctoidChannelData>
    {
        IFunctoidActionViewModelFactory _actionFactory;
        IExtractor<IFunctoidActionViewModel, FunctoidActionData> _actionExtractor;

        public FunctoidChannelViewModelDataSerializer(IFunctoidActionViewModelFactory actionFactory, IExtractor<IFunctoidActionViewModel, FunctoidActionData> actionExtractor)
        {
            _actionFactory = actionFactory;
            _actionExtractor = actionExtractor;
        }

        public FunctoidChannelData ExtractData(IFunctoidChannelViewModel source)
        {
            return new FunctoidChannelData()
            {
                Index = source.Index,
                Name = source.Name,
                Actions = source.Actions?.ToList().ConvertAll(input => _actionExtractor.ExtractData(input)),
                IsEnabled = source.IsEnabled,
                IsExpanded = source.IsExpanded,
                BindedInput = source.SelectedInput?.FormatedName,
            };
        }

        public void ApplyData(FunctoidChannelData data, IFunctoidChannelViewModel target)
        {
            target.Index = data.Index;
            target.Name = data.Name;
            target.Actions = new ObservableCollection<IFunctoidActionViewModel>(data.Actions.ConvertAll(d => _actionFactory.Build(d)));
            target.IsEnabled = data.IsEnabled;
            target.IsExpanded = data.IsExpanded;
            target.RefreshInputs();
            target.TryBindInput(data.BindedInput);
        }
    }
}
﻿using System;
using System.Collections.ObjectModel;
using MoonBurst.Api.Client;

namespace MoonBurst.ViewModel.Interface
{
    public interface ILayoutChannelViewModel : IViewModel
    {
        ObservableCollection<IChannelActionViewModel> Actions { get; set; }
        int Index { get; set; }
        bool IsEnabled { get; set; }
        bool IsExpanded { get; set; }
        bool IsTriggered { get; set; }
        bool IsLocked { get; set; }
        string Name { get; set; }
        IDeviceInputViewModel SelectedInput { get; set; }

        void RefreshInputs();
        void TryBindInput(string bindedInput);

        event EventHandler DeleteRequested;
    }
}
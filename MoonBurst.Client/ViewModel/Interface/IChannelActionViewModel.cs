using System;
using System.Collections.Generic;
using System.Windows.Input;
using MoonBurst.Api.Client;
using MoonBurst.Api.Enums;
using MoonBurst.Api.Helper;

namespace MoonBurst.ViewModel.Interface
{
    public interface IChannelActionViewModel : IViewModel
    {
        string DisplayName { get; }
        string DisplayNameToolTip { get; }

        IList<FormatedNote> AvailableNotes { get; }
        ChannelCommand Command { get; set; }
        int Data1 { get; set; }
        int Data2 { get; set; }
        int Delay { get; set; }
        bool IsEnabled { get; set; }
        bool IsExpanded { get; set; }
        bool IsTriggered { get; set; }
        bool IsLocked { get; set; }
        bool IsParentChannelLocked { get; set; }
        bool IsLockedOrParentChannelLocked { get; }
        int MidiChannel { get; set; }
        ICommand OnDeleteActionCommand { get; set; }
        ICommand OnToggleActionCommand { get; set; }
        ICommand OnTriggerActionCommand { get; set; }
        FootTrigger Trigger { get; set; }

        void TriggerAction();

        event EventHandler DeleteRequested;
    }
}
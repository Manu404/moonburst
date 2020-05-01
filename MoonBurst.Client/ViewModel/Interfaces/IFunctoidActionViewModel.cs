using System.Collections.Generic;
using System.Windows.Input;
using MoonBurst.Api.Enums;
using MoonBurst.Core;
using MoonBurst.Core.Helper;

namespace MoonBurst.ViewModel.Interfaces
{
    public interface IFunctoidActionViewModel : IViewModel
    {
        string DisplayName { get; }
        string DisplayNameToolTip { get; }

        List<MusicalNote> AvailableNotes { get; }
        ChannelCommand Command { get; set; }
        int Data1 { get; set; }
        int Data2 { get; set; }
        int Delay { get; set; }
        bool IsEnabled { get; set; }
        bool IsExpanded { get; set; }
        bool IsTriggered { get; set; }
        bool IsLocked { get; set; }
        bool IsChannelLocked { get; set; }
        bool IsLockedOrChannelLocked { get; }
        int MidiChannel { get; set; }
        ICommand OnDeleteActionCommand { get; set; }
        ICommand OnToggleActionCommand { get; set; }
        ICommand OnTriggerActionCommand { get; set; }
        FootTrigger Trigger { get; set; }

        void OnTriggerAction();
    }
}
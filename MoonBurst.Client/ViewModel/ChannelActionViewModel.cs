using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Enums;
using MoonBurst.Api.Gateway.Midi;
using MoonBurst.Api.Helper;
using MoonBurst.Helper;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{
    public class ChannelActionViewModel : ViewModelBase, IChannelActionViewModel
    {
        private readonly INoteHelper _noteHelper;
        private readonly IDynamicsHelper _dynamicsHelper;
        private readonly IMidiGateway _midiGateway;

        private ChannelCommand _command;
        private bool _isTriggered;
        private bool _forceNumericMode;
        private bool _isLocked;
        private bool _isParentChannelLocked;

        public string EnableStatusString => IsEnabled ? "disabled" : "enabled";
        public string LockedStatusString => IsLocked ? "locked" : "unlocked";
        public string StatusString => $"({EnableStatusString}/{LockedStatusString})";
        public string DisplayName => $"On {Trigger}\n{Command}({MidiChannel},{Data1},{Data2})";
        public string DisplayNameToolTip => $"On {Trigger}\nMessage: {Command}\nChannel: {MidiChannel}\nData1: {Data1}\nData2: {Data2}";
        public string EnableTooltip => IsEnabled ? "disable" : "enable";
        public string ChannelHeaderActionToggleTooltip => $"{DisplayNameToolTip}\n\nLeft-click to manually trigger\nRight-click to {EnableTooltip}";

        private void RefreshTitle()
        {
            RaisePropertyChanged("DisplayName");
            RaisePropertyChanged("DisplayNameToolTip");
            RaisePropertyChanged("ChannelHeaderActionToggleTooltip");
        }

        public FootTrigger Trigger { get; set; }
        public int MidiChannel { get; set; }
        public int Data1 { get; set; }
        public int Data2 { get; set; }
        public ChannelCommand Command
        {
            get => _command;
            set
            {
                _command = value;
                RaisePropertyChanged();
                IsMusicalMode = (Command == ChannelCommand.NoteOn || Command == ChannelCommand.NoteOff) && !ForceNumericMode;
            }
        }
        public int Delay { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsLockedOrParentChannelLocked");
            }
        }

        public bool IsParentChannelLocked
        {
            get => _isParentChannelLocked;
            set
            {
                _isParentChannelLocked = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsLockedOrParentChannelLocked));
            }
        }

        public bool IsLockedOrParentChannelLocked => IsLocked || IsParentChannelLocked;

        public bool IsExpanded { get; set; }

        public bool IsTriggered
        {
            get => _isTriggered;
            set
            {
                _isTriggered = value;
                RaisePropertyChanged();
                _isTriggered = !value;
                RaisePropertyChanged();
            }
        }

        public bool IsMusicalMode { get; set; }

        public bool ForceNumericMode
        {
            get => _forceNumericMode;
            set
            {
                _forceNumericMode = value;
                IsMusicalMode = (Command == ChannelCommand.NoteOn || Command == ChannelCommand.NoteOff) && !ForceNumericMode;
                RaisePropertyChanged();
            }
        }

        public IList<FormatedNote> AvailableNotes => _noteHelper.AvailableNotes;
        public IList<Dynamic> AvailableDynamics => _dynamicsHelper.AvailableDynamics;

        public ICommand OnDeleteActionCommand { get; set; }
        public ICommand OnToggleActionCommand { get; set; }
        public ICommand OnTriggerActionCommand { get; set; }
        public ICommand OnLockActionCommand { get; set; }

        public event EventHandler DeleteRequested;

        public ChannelActionViewModel(INoteHelper noteHelper, IDynamicsHelper dynamicsHelper, IMidiGateway midiGateway)
        {
            OnDeleteActionCommand = new RelayCommand(OnDelete);
            OnTriggerActionCommand = new RelayCommand(TriggerAction);
            OnToggleActionCommand = new RelayCommand(OnToggle);
            OnLockActionCommand = new RelayCommand(OnLock);

            _noteHelper = noteHelper;
            _dynamicsHelper = dynamicsHelper;
            _midiGateway = midiGateway;

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Contains("DisplayName") ||
                    args.PropertyName.Contains("ChannelHeaderActionToggleTooltip")) return; // avoid self trigger
                RefreshTitle();
            };
        }

        private void OnLock()
        {
            IsLocked = !IsLocked;
        }

        private void OnToggle()
        {
            IsEnabled = !IsEnabled;
        }

        public void TriggerAction()
        {
            if (Application.Current.Dispatcher != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { IsTriggered = true; }));

            _midiGateway.Trigger(new MidiTriggerData()
            {
                Command = this.Command,
                Data1 = this.Data1,
                Data2 = this.Data2,
                MidiChannel = this.MidiChannel,
                Delay = this.Delay
            });
        }

        private async void OnDelete()
        {
            var result = await ConfirmationHelper.RequestConfirmationBeforeDeletation();
            if (result is bool boolResult && boolResult)
            {
                DeleteRequested?.Invoke(this, new EventArgs());
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Enums;
using MoonBurst.Api.Gateways;
using MoonBurst.Core.Helper;
using MoonBurst.Helper;
using MoonBurst.Model.Messages;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel
{
    public class FunctoidActionViewModel : ViewModelBase, IFunctoidActionViewModel
    {
        private IMessenger _messenger;
        private IMusicalNoteHelper _noteHelper;
        private IDynamicsHelper _dynamicsHelper;
        private IMidiGateway _midiGateway;

        private bool _isEnabled;
        private bool _isExpanded;
        private FootTrigger _trigger;
        private ChannelCommand _command;
        private int _data2;
        private int _data1;
        private int _midiChannel;
        private int _delay;
        private bool _isTriggered;
        private bool _isMusicalMode;
        private bool _forceNumericMode;

        public string DisplayName
        {
            get => $"On {Trigger}\n{Command}({MidiChannel},{Data1},{Data2})";
        }

        public string DisplayNameToolTip
        {
            get => $"On {Trigger}\nMessage: {Command}\nChannel: {MidiChannel}\nData1: {Data1}\nData2: {Data2}";
        }

        private void RefreshTitle()
        {
            RaisePropertyChanged("DisplayName");
            RaisePropertyChanged("DisplayNameToolTip");
        }

        public FootTrigger Trigger
        {
            get => _trigger;
            set
            {
                _trigger = value;
                RaisePropertyChanged();
                RefreshTitle();
            }
        }

        public int MidiChannel
        {
            get => _midiChannel;
            set
            {
                _midiChannel = value;
                RaisePropertyChanged();
                RefreshTitle();
            }
        }

        public int Data1
        {
            get => _data1;
            set
            {
                _data1 = value;
                RaisePropertyChanged();
                RefreshTitle();
            }
        }

        public int Data2
        {
            get => _data2;
            set
            {
                _data2 = value;
                RaisePropertyChanged();
                RefreshTitle();
            }
        }

        public ChannelCommand Command
        {
            get => _command;
            set
            {
                _command = value;
                RaisePropertyChanged();
                IsMusicalMode = (Command == ChannelCommand.NoteOn || Command == ChannelCommand.NoteOff) && !ForceNumericMode;
                RefreshTitle();
            }
        }

        public int Delay
        {
            get => _delay;
            set
            {
                _delay = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                RaisePropertyChanged();
            }
        }

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

        public bool IsMusicalMode
        {
            get => _isMusicalMode;
            set
            {
                _isMusicalMode = value;
                RaisePropertyChanged();
            }
        }

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

        public List<MusicalNote> AvailableNotes => _noteHelper.AvailableNotes;
        public List<Dynamic> AvailableDynamics => _dynamicsHelper.AvailableDynamics;

        public ICommand OnDeleteActionCommand { get; set; }
        public ICommand OnToggleActionCommand { get; set; }
        public ICommand OnTriggerActionCommand { get; set; }

        public FunctoidActionViewModel(IMessenger messenger, IMusicalNoteHelper noteHelper, IDynamicsHelper dynamicsHelper, IMidiGateway midiGateway)
        {
            OnDeleteActionCommand = new RelayCommand(OnDelete);
            OnTriggerActionCommand = new RelayCommand(OnTriggerAction);
            OnToggleActionCommand = new RelayCommand(OnToggle);

            _messenger = messenger;
            _noteHelper = noteHelper;
            _dynamicsHelper = dynamicsHelper;
            _midiGateway = midiGateway;
        }

        private void OnToggle()
        {
            this.IsEnabled = !this.IsEnabled;
        }

        public void OnTriggerAction()
        {
            if (Application.Current.Dispatcher != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { this.IsTriggered = true; }));

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
            var result = await ConfirmationHelper.RequestConfirmationForDeletation();
            if (result is bool boolResult && boolResult)
            {
                _messenger.Send(new DeleteFunctoidActionMessage(this));
            }
        }
    }
}
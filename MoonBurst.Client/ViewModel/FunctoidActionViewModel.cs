using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Core;
using MoonBurst.Core.Helper;
using MoonBurst.Model;
using MoonBurst.Model.Messages;

namespace MoonBurst.ViewModel
{


    public partial class FunctoidActionViewModel : ViewModelBase, IFunctoidActionViewModel
    {
        private IArduinoGateway _arduinoGateway;
        private IMessenger _messenger;
        private IMusicalNoteHelper _noteHelper;
        private IDynamicsHelper _dynamicsHelper;

        private bool _isEnabled;
        private bool _isExpanded;
        private string _actionName;
        private FootTrigger _trigger;
        private ChannelCommand _command;
        private int _data2;
        private int _data1;
        private int _midiChannel;
        private int _delay;
        private bool _isTriggered;
        private bool _isMusicalMode;
        private bool _forceNumericMode;

        public string ActionName
        {
            get => $"On {Trigger} \n{Command}({Data1},{Data2}) Channel({MidiChannel})";
        }

        public FootTrigger Trigger
        {
            get => _trigger;
            set
            {
                _trigger = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ActionName");
            }
        }

        public int MidiChannel
        {
            get => _midiChannel;
            set
            {
                _midiChannel = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ActionName");
            }
        }

        public int Data1
        {
            get => _data1;
            set
            {
                _data1 = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ActionName");
            }
        }

        public int Data2
        {
            get => _data2;
            set
            {
                _data2 = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ActionName");
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
                RaisePropertyChanged("ActionName");
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
                _isTriggered = true;
                RaisePropertyChanged();
                _isTriggered = false;
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

        public FunctoidActionViewModel(IMessenger messenger, IArduinoGateway arduinoGateway, IMusicalNoteHelper noteHelper, IDynamicsHelper dynamicsHelper)
        {
            OnDeleteActionCommand = new RelayCommand(OnDelete);
            OnTriggerActionCommand = new RelayCommand(OnTriggerAction);
            OnToggleActionCommand = new RelayCommand(OnToggle);

            _arduinoGateway = arduinoGateway;
            _messenger = messenger;
            _noteHelper = noteHelper;
            _dynamicsHelper = dynamicsHelper;
        }

        private void OnToggle()
        {
            this.IsEnabled = !this.IsEnabled;
        }

        public void OnTriggerAction()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.IsTriggered = true;
            }));
            _messenger.Send(new TriggeredActionMessage() { Data = this });
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
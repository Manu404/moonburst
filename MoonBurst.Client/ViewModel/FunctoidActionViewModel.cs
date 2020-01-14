using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Core;
using MoonBurst.Core.Helper;
using MoonBurst.Model;
using MoonBurst.Model.Messages;
using MoonBurst.Model.Parser;

namespace MoonBurst.ViewModel
{
    public class DeviceInputViewModel : ViewModelBase
    {
        public IArduinoPort Port { get; set; }
        public IDeviceDefinition Device { get; set; }
        public IDeviceInput Input { get; set; }

        public IFootswitchState State
        {
            get => _state;
            set
            {
                _state = value;
                RaisePropertyChanged();
            }
        }

        public string PortName => $"Port: {Port.Position + 1}";
        public string DeviceName => $"Device: {Device.Name}";
        public string ControllerName => $"Controller: {ControllerNameWithoutHeader}";
        public string ControllerNameWithoutHeader => $"{Input.Name} ({Input.Position + 1})";

        public string FormatedName => $"{PortName}\n{DeviceName}\n{ControllerName}";

        private IMessenger _messenger;
        private IFootswitchState _state;

        public DeviceInputViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            _messenger.Register<ControllerStateMessage>(this, OnControllerStateChanged);
        }

        private void OnControllerStateChanged(ControllerStateMessage obj)
        {
            if (obj.Port != Port.Position) return;
            foreach(var state in obj.States)
                if (state.Index == Input.Position)
                {
                    State = state;
                    return;
                }
        }
    }

    public class FunctoidActionViewModel : ViewModelBase
    {
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
                RaisePropertyChanged("ActionName");
            }
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

        public int Delay
        {
            get => _delay;
            set
            {
                _delay = value;
                RaisePropertyChanged();
            }
        }

        public string ActionName
        {
            get => $"On {Trigger} \n{Command}({Data1},{Data2}) Channel({MidiChannel})";
        }

        public ICommand OnDeleteActionCommand { get; set; }
        public ICommand OnToggleActionCommand { get; set; }
        public ICommand OnTriggerActionCommand { get; set; }

        private IArduinoGateway _arduinoGateway;
        private IMessenger _messenger;

        public FunctoidActionViewModel(IMessenger messenger, IArduinoGateway arduinoGateway)
        {
            OnDeleteActionCommand = new RelayCommand(OnDelete);
            OnTriggerActionCommand = new RelayCommand(OnTriggerAction);
            OnToggleActionCommand = new RelayCommand(OnToggle);

            _arduinoGateway = arduinoGateway;
            _messenger = messenger;
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
            _messenger.Send(new TriggeredActionMessage() { Data = this.GetData() });
        }

        private async void OnDelete()
        {
            var result = await ConfirmationHelper.RequestConfirmationForDeletation();
            if (result is bool boolResult && boolResult)
            {
                _messenger.Send(new DeleteFunctoidActionMessage(this));
            }
        }

        public FunctoidActionViewModel(FunctoidActionData data, IArduinoGateway arduinoGateway, IMessenger messenger) : this(messenger, arduinoGateway)
        {
            this.MidiChannel = data.MidiChannel;
            this.Data2 = data.Data2;
            this.Data1 = data.Data1;
            this.Command = data.Command;
            this.Trigger = data.Trigger;
            this.IsEnabled = data.IsEnabled;
            this.IsExpanded = data.IsExpanded;
        }

        public FunctoidActionData GetData()
        {
            return new FunctoidActionData()
            {
                MidiChannel = this.MidiChannel,
                Data1 = this.Data1,
                Data2 = this.Data2,
                Command = this.Command,
                Trigger = this.Trigger,
                IsEnabled = this.IsEnabled,
                IsExpanded = this.IsExpanded
            };
        }

        public class FunctoidActionData
        {
            public int MidiChannel { get; set; }
            public int Data1 { get; set; }
            public int Data2 { get; set; }
            public ChannelCommand Command { get; set; }
            public FootTrigger Trigger { get; set; }
            public bool IsEnabled { get; set; }
            public bool IsExpanded { get; set; }
        }
    }
}
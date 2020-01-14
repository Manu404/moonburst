using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Core;
using MoonBurst.Core.Helper;
using MoonBurst.Model.Messages;

namespace MoonBurst.ViewModel
{
    public class FunctoidChannelViewModel : ViewModelBase
    {
        private IArduinoGateway _arduinoGateway;
        private IMessenger _messenger;
        private string _lastInput;
        
        private int _index;
        private string _name;
        private bool _isEnabled;
        private bool _isExpanded;
        private bool _isTriggered;
        private DeviceInputViewModel _selectedInput;
        
        public DeviceInputViewModel SelectedInput
        {
            get => _selectedInput;
            set
            {
                _selectedInput = value;
                if (value != null)
                    _lastInput = value.FormatedName;
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

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                RaisePropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
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

        public int ArduinoBitMask { get; set; }
        public ObservableCollection<FunctoidActionViewModel> Actions { get; set; }
        public ObservableCollection<DeviceInputViewModel> AvailableInputs { get; set; }

        public ICommand OnDeleteCommand { get; set; }
        public ICommand OnAddActionCommand { get; set; }
        public ICommand OnTriggerCommand { get; set; }
        public ICommand OnToggleCommand { get; set; }
        public ICommand OnExpandActionsCommand { get; set; }
        public ICommand OnCollapseActionsCommand { get; set; }

        public FunctoidChannelViewModel(IMessenger messenger)
        {
            _messenger = messenger;

            OnDeleteCommand = new RelayCommand(OnDelete);
            OnAddActionCommand = new RelayCommand(OnAddAction);
            OnTriggerCommand = new RelayCommand(OnTrigger);
            OnToggleCommand = new RelayCommand(()  => this.IsEnabled = !this.IsEnabled);

            OnExpandActionsCommand = new RelayCommand(() => ToggleAction(true));
            OnCollapseActionsCommand = new RelayCommand(() => ToggleAction(false));

            Actions = new ObservableCollection<FunctoidActionViewModel>();

            _messenger.Register<DeleteFunctoidActionMessage>(this, OnDeleteAction);
            _messenger.Register<PortConfigChangedMessage>(this, OnPortConfigChanged);
            _messenger.Register<ControllerStateMessage>(this, OnControllerStateChanged);

            AvailableInputs = new ObservableCollection<DeviceInputViewModel>();
        }

        private void OnControllerStateChanged(ControllerStateMessage obj)
        {
            if (this.SelectedInput == null) return;
            if (obj.Port != this.SelectedInput.Port.Position) return;
            foreach (var state in obj.States)
                if (state.Index == this.SelectedInput.Input.Position)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.IsTriggered = true;
                    }));
                    this.Actions.Where(a => (int)a.Trigger == (int)state.State).ToList().ForEach(a => a.OnTriggerAction());
                    return;
                }
        }

        private void ToggleAction(bool isExpanded)
        {
            foreach (var action in this.Actions)
                action.IsExpanded = isExpanded;
        }

        private void OnPortConfigChanged(PortConfigChangedMessage obj)
        {
            RefreshInputs();
        }

        public void RefreshInputs()
        {
            AvailableInputs.Clear();
            if (_arduinoGateway == null) return;
            foreach (var port in _arduinoGateway.Ports)
            {
                if (port.ConnectedDevice == null) continue;

                foreach (var input in port.ConnectedDevice.GetInputs())
                {
                    AvailableInputs.Add(new DeviceInputViewModel(_messenger)
                    {
                        Device = port.ConnectedDevice,
                        Input = input,
                        Port = port,
                    });
                }
            }

            this.SelectedInput = AvailableInputs.FirstOrDefault(d => d.FormatedName == _lastInput);
        }

        private void OnTrigger()
        {
            this.IsTriggered = true;
        }

        private void OnDeleteAction(DeleteFunctoidActionMessage obj)
        {
            Actions.Remove(obj.Item);
        }

        private void OnAddAction()
        {
            Actions.Add(new FunctoidActionViewModel(_messenger, _arduinoGateway));
        }

        public FunctoidChannelViewModel(FunctoidChannelData data, IArduinoGateway arduinoGateway, IMessenger messenger) : this(messenger)
        {
            this.Index = data.Index;
            this.Name = data.Name;
            this.Actions = new ObservableCollection<FunctoidActionViewModel>(data.Actions.ConvertAll(d => new FunctoidActionViewModel(d, this._arduinoGateway, _messenger)));
            this.IsEnabled = data.IsEnabled;
            this.IsExpanded = data.IsExpanded;
            this._arduinoGateway = arduinoGateway;
            this._lastInput = data.BindedInput;
            this._messenger = messenger;
        }

        private async void OnDelete()
        {
            var result = await ConfirmationHelper.RequestConfirmationForDeletation();
            if (result is bool boolResult && boolResult)
            {
                _messenger.Send(new DeleteFunctoidChannelMessage(this));
            }
        }

        public FunctoidChannelData GetData()
        {
            return new FunctoidChannelData()
            {
                Index = this.Index,
                Name = this.Name,
                Actions = this.Actions?.ToList().ConvertAll(input => input.GetData()),
                IsEnabled = this.IsEnabled,
                IsExpanded = this.IsExpanded,
                BindedInput = this.SelectedInput?.FormatedName,
            };
        }


        public class FunctoidChannelData
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public bool IsEnabled { get; set; }
            public bool IsExpanded { get; set; }
            public string BindedInput { get; set; }

            public List<FunctoidActionViewModel.FunctoidActionData> Actions { get; set; }

            public FunctoidChannelData()
            {
                Actions = new List<FunctoidActionViewModel.FunctoidActionData>();
            }
        }
    }
}
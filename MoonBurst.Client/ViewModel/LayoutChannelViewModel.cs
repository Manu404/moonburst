using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Client;
using MoonBurst.Api.Gateway.Arduino;
using MoonBurst.Api.Gateway.Serial;
using MoonBurst.Api.Hardware.Parser;
using MoonBurst.Helper;
using MoonBurst.ViewModel.Factory;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{
    public class LayoutChannelViewModel : ViewModelBase, ILayoutChannelViewModel
    {
        private readonly IArduinoGateway _arduinoGateway;
        private readonly IChannelActionViewModelFactory _actionFactory;
        private readonly IFactory<IDeviceInputViewModel> _deviceInputViewModelFactory;
        private readonly ISerialGateway _serialGateway;

        private IDeviceInputViewModel _selectedInput;
        private bool _isTriggered;
        private string _lastInput;
        
        public IDeviceInputViewModel SelectedInput
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

        public int Index { get; set; }
        public string Name { get; set; }

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
        public bool IsLocked { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsExpanded { get; set; }

        public event EventHandler DeleteRequested;

        public ObservableCollection<IChannelActionViewModel> Actions { get; set; }
        public ObservableCollection<IDeviceInputViewModel> AvailableInputs { get; set; }

        public ICommand OnDeleteCommand { get; set; }
        public ICommand OnAddActionCommand { get; set; }
        public ICommand OnTriggerCommand { get; set; }
        public ICommand OnToggleCommand { get; set; }
        public ICommand OnExpandActionsCommand { get; set; }
        public ICommand OnCollapseActionsCommand { get; set; }
        public ICommand OnToggleLockChannelCommand { get; set; }

        public LayoutChannelViewModel(IArduinoGateway arduinoGateway, 
            IChannelActionViewModelFactory actionFactory,
            IFactory<IDeviceInputViewModel> deviceInputViewModelFactory,
            ISerialGateway serialGateway)
        {
            _actionFactory = actionFactory;
            _arduinoGateway = arduinoGateway;
            _deviceInputViewModelFactory = deviceInputViewModelFactory;
            _serialGateway = serialGateway;

            OnDeleteCommand = new RelayCommand(OnDelete);
            OnAddActionCommand = new RelayCommand(OnAddAction);
            OnTriggerCommand = new RelayCommand(OnTrigger);
            OnToggleCommand = new RelayCommand(()  => this.IsEnabled = !this.IsEnabled);
            OnToggleLockChannelCommand = new RelayCommand(OnToggleLock);
            OnExpandActionsCommand = new RelayCommand(() => OnExpandCollapse(true));
            OnCollapseActionsCommand = new RelayCommand(() => OnExpandCollapse(false));

            Actions = new ObservableCollection<IChannelActionViewModel>();
            AvailableInputs = new ObservableCollection<IDeviceInputViewModel>();

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _serialGateway.OnTrigger += OnControllerStateChanged;
            _arduinoGateway.ConnectedDevicesChanged += OnConnectedDevicesChanged;

            foreach (var action in Actions)
            {
                action.DeleteRequested += OnDeleteRequested;
            }
        }

        private void OnDeleteRequested(object sender, EventArgs e)
        {
            if (sender is IChannelActionViewModel action)
            {
                action.DeleteRequested -= OnDeleteRequested;
                Actions.Remove(action);
            }
        }

        private void OnConnectedDevicesChanged(object sender, EventArgs e)
        {
            RefreshInputs();
        }

        private void OnToggleLock()
        {
            this.IsLocked = !this.IsLocked;
            foreach (var a in Actions)
                a.IsParentChannelLocked = this.IsLocked;
        }

        private void OnControllerStateChanged(object sender, ControllerStateEventArgs obj)
        {
            if (SelectedInput == null) return;
            if (obj.Port != SelectedInput.Port.Position) return;
            foreach (var state in obj.States)
                if (state.Index == SelectedInput.Input.Position)
                {
                    Actions.Where(a => (int)a.Trigger == (int)((FootswitchState)state).States ).ToList().ForEach(a => a.TriggerAction());
                    return;
                }
        }

        private void OnExpandCollapse(bool isExpanded)
        {
            foreach (var action in Actions)
                action.IsExpanded = isExpanded;
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
                    var deviceInput = _deviceInputViewModelFactory.Build();
                    deviceInput.Device = port.ConnectedDevice;
                    deviceInput.Input = input;
                    deviceInput.Port = port;
                    AvailableInputs.Add(deviceInput);
                }
            }

            TryBindInput(_lastInput);
        }

        private void OnTrigger()
        {
            IsTriggered = true;
        }
        
        private void OnAddAction()
        {
            var action = _actionFactory.Build();
            action.DeleteRequested += OnDeleteRequested;
            Actions.Add(action);
        }

        private async void OnDelete()
        {
            var result = await ConfirmationHelper.RequestConfirmationBeforeDeletation();
            if (result is bool boolResult && boolResult)
            {
                DeleteRequested?.Invoke(this, new EventArgs());
            }
        }

        public void TryBindInput(string bindedInput)
        {
            SelectedInput = AvailableInputs.FirstOrDefault(d => d.FormatedName == bindedInput);
        }

    }
}
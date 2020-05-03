using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Gateways;
using MoonBurst.Core;
using MoonBurst.Core.Helper;
using MoonBurst.Helper;
using MoonBurst.Model.Messages;
using MoonBurst.ViewModel.Factories;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel
{
    public class LayoutChannelViewModel : ViewModelBase, ILayoutChannelViewModel
    {
        private readonly IArduinoGateway _arduinoGateway;
        private readonly IMessenger _messenger;
        private readonly IChannelActionViewModelFactory _actionFactory;
        private readonly IFactory<IDeviceInputViewModel> _deviceInputViewModelFactory;
        private readonly ISerialGateway _serialGateway;
        private string _lastInput;
        
        private int _index;
        private string _name;
        private bool _isEnabled;
        private bool _isExpanded;
        private bool _isTriggered;
        private bool _isLocked;
        private IDeviceInputViewModel _selectedInput;

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

        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;
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

        public ObservableCollection<IChannelActionViewModel> Actions { get; set; }
        public ObservableCollection<IDeviceInputViewModel> AvailableInputs { get; set; }

        public int ArduinoBitMask { get; set; }

        public ICommand OnDeleteCommand { get; set; }
        public ICommand OnAddActionCommand { get; set; }
        public ICommand OnTriggerCommand { get; set; }
        public ICommand OnToggleCommand { get; set; }
        public ICommand OnExpandActionsCommand { get; set; }
        public ICommand OnCollapseActionsCommand { get; set; }
        public ICommand OnToggleLockChannelCommand { get; set; }

        public LayoutChannelViewModel(IMessenger messenger, 
            IArduinoGateway arduinoGateway, 
            IChannelActionViewModelFactory actionFactory,
            IFactory<IDeviceInputViewModel> deviceInputViewModelFactory,
            ISerialGateway serialGateway)
        {
            _messenger = messenger;
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

            _messenger.Register<DeleteChannelActionMessage>(this, OnDeleteAction);
            _messenger.Register<PortConfigChangedMessage>(this, OnPortConfigChanged);

            AvailableInputs = new ObservableCollection<IDeviceInputViewModel>();

            _serialGateway.OnTrigger += OnControllerStateChanged;
        }

        private void OnToggleLock()
        {
            this.IsLocked = !this.IsLocked;
            foreach (var a in Actions)
                a.IsChannelLocked = this.IsLocked;
        }

        private void OnControllerStateChanged(object sender, ControllerStateEventArgs obj)
        {
            if (this.SelectedInput == null) return;
            if (obj.Port != this.SelectedInput.Port.Position) return;
            foreach (var state in obj.States)
                if (state.Index == this.SelectedInput.Input.Position)
                {
                    this.Actions.Where(a => (int)a.Trigger == (int)state.State).ToList().ForEach(a => a.OnTriggerAction());
                    return;
                }
        }

        private void OnExpandCollapse(bool isExpanded)
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
            this.IsTriggered = true;
        }

        private void OnDeleteAction(DeleteChannelActionMessage obj)
        {
            Actions.Remove(obj.Item);
        }

        private void OnAddAction()
        {
            Actions.Add(_actionFactory.Build());
        }

        private async void OnDelete()
        {
            var result = await ConfirmationHelper.RequestConfirmationBeforeDeletation();
            if (result is bool boolResult && boolResult)
            {
                _messenger.Send(new DeleteChannelMessage(this));
            }
        }

        public void TryBindInput(string bindedInput)
        {
            this.SelectedInput = AvailableInputs.FirstOrDefault(d => d.FormatedName == bindedInput);
        }
    }
}
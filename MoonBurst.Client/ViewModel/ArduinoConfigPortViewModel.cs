using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Client;
using MoonBurst.Api.Gateway.Arduino;
using MoonBurst.Api.Hardware.Description;
using MoonBurst.Model;
using MoonBurst.Model.Message;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{
    public class ArduinoConfigPortViewModel : ViewModelBase, IArduinoConfigPortViewModel
    {
        private readonly IFactory<IDeviceInputViewModel> _deviceInputViewModelFactory;
        private readonly IMessenger _messenger;
        private bool _isConnected;
        private bool _isEnabled;

        public IDeviceDefinition ConnectedDevice
        {
            get => Port.ConnectedDevice;
            set
            {
                Port.ConnectedDevice = value;
                IsConnected = value != null;
                RaisePropertyChanged("PortName");
                RaisePropertyChanged("ConnectedDeviceName");
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                RaisePropertyChanged();
                RaisePropertyChanged("PortName");
            }
        }

        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                RaisePropertyChanged();
            }
        }
        
        public IArduinoPort Port { get; }

        public int Position => Port.Position;

        public ObservableCollection<IDeviceDefinition> AvailableDevices { get; }
        public ObservableCollection<IDeviceInputViewModel> AvailableInputs { get; }

        public string PortName => $"Port {Position + 1}" + (IsEnabled ? "" : "(muted)");
        public string ConnectedDeviceName => ConnectedDevice != null ? ConnectedDevice.Name : "Disconnected";
        
        public ICommand ConnectCommand { get; }
        public ICommand DisableCommand { get; }

        public ArduinoConfigPortViewModel(IArduinoPort port, IMessenger messenger, IFactory<IDeviceInputViewModel> deviceInputViewModelFactory)
        {
            IsConnected = false;
            _messenger = messenger;
            _deviceInputViewModelFactory = deviceInputViewModelFactory;

            ConnectCommand = new RelayCommand<string>(Connect);
            DisableCommand = new RelayCommand(ToggleEnable);

            AvailableDevices = new ObservableCollection<IDeviceDefinition>();
            
            Port = port;
            foreach (var device in port.AvailableDevices)
                AvailableDevices.Add(device);

            AvailableInputs = new ObservableCollection<IDeviceInputViewModel>();
        }

        public void ToggleEnable()
        {
            IsEnabled = !IsEnabled;
        }

        public void Connect(string obj)
        {
            this.IsConnected = true;
            ConnectedDevice = this.AvailableDevices.FirstOrDefault(s => s.Name == obj);
            RefreshInputs();
            _messenger.Send(new PortConfigChangedMessage());
        }

        public void RefreshInputs()
        {
            AvailableInputs.Clear();
            if (ConnectedDevice == null) return;

            foreach (var input in ConnectedDevice.GetInputs())
            {
                var deviceInput = _deviceInputViewModelFactory.Build();
                deviceInput.Device = ConnectedDevice;
                deviceInput.Input = input;
                deviceInput.Port = Port;
                AvailableInputs.Add(deviceInput);
            }
        }

        public ArduinoPortConfigModel GetModel()
        {
            return new ArduinoPortConfigModel()
            {
                Position = this.Position,
                ConnectedDevice = this.ConnectedDeviceName,
                IsEnabled = this.IsEnabled,
            };
        }
    }

}
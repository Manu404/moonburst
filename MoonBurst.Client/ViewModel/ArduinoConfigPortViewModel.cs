using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Api.Hardware;
using MoonBurst.Model;
using MoonBurst.Model.Messages;
using MoonBurst.Model.Serializable;

namespace MoonBurst.ViewModel
{
    public class ArduinoConfigPortViewModel : ViewModelBase
    {
        private IMessenger _messenger;
        private bool _isConnected;
        private bool _isEnabled;

        private IDeviceDefinition ConnectedDevice
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
        public ObservableCollection<DeviceInputViewModel> AvailableInputs { get; }

        public string PortName => $"Port {Position + 1}" + (IsEnabled ? "" : "(muted)");
        public string ConnectedDeviceName => ConnectedDevice != null ? ConnectedDevice.Name : "Disconnected";
        
        public ICommand ConnectCommand { get; }
        public ICommand DisableCommand { get; }

        public ArduinoConfigPortViewModel(IArduinoPort port, IMessenger messenger)
        {
            Port = port;
            IsConnected = false;
            _messenger = messenger;

            ConnectCommand = new RelayCommand<string>(Connect);
            DisableCommand = new RelayCommand(ToggleEnable);

            AvailableDevices = new ObservableCollection<IDeviceDefinition>();
            foreach (var device in port.AvailableDevices)
                AvailableDevices.Add(device);

            AvailableInputs = new ObservableCollection<DeviceInputViewModel>();
        }

        private void ToggleEnable()
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
                AvailableInputs.Add(new DeviceInputViewModel(_messenger)
                {
                    Device = ConnectedDevice,
                    Input = input,
                    Port = Port,
                });
            }
        }

        public ArduinoPortConfigData GetData()
        {
            return new ArduinoPortConfigData()
            {
                Position = this.Position,
                ConnectedDevice = this.ConnectedDeviceName,
                IsEnabled = this.IsEnabled,
            };
        }
    }

}
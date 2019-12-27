using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using MoonBurst.Core;
using MoonBurst.Model;

namespace MoonBurst.ViewModel
{
    public class HardwareConfigurationViewModel : ViewModelBase, IFileSerializableType<HardwareConfigurationViewModel.HardwareConfigurationData>
    {
        private IMidiGateway _midiGateway;
        private ISerialGateway _serialGateway;
        private bool _isMidiConnected;
        private bool _isComConnected;

        public int SelectedSpeed
        {
            get => _serialGateway.CurrentSpeed;
            set
            {
                _serialGateway.CurrentSpeed = value;
                RaisePropertyChanged();
            }
        }

        public InputCOMPort SelectedComPort
        {
            get => _serialGateway.CurrentPort;
            set
            {
                _serialGateway.CurrentPort = value;
                RaisePropertyChanged();
            }
        }

        public OutputMidiDevice SelectedOutputMidiDevice
        {
            get => _midiGateway.SelectedOutput;
            set
            {
                _midiGateway.SelectedOutput = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<OutputMidiDevice> OutputMidiDevices { get; set; }
        public ObservableCollection<InputCOMPort> InputComPorts { get; set; }
        public ObservableCollection<int> SupportedBaudRates { get; set; }

        public bool IsMidiConnected
        {
            get => _isMidiConnected;
            set
            {
                _isMidiConnected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsComConnected
        {
            get => _isComConnected;
            set
            {
                _isComConnected = value;
                RaisePropertyChanged();
            }
        }

        public ICommand OnSendMidiTestCommand { get; set; }
        public ICommand OnRefreshMidiCommand { get; set; }
        public ICommand OnRefreshComCommand { get; set; }
        public ICommand OnConnectMidiCommand { get; set; }
        public ICommand OnConnectComCommand { get; set; }

        public ICommand LoadConfigCommand { get; set; }
        public ICommand SaveConfigCommand { get; set; }
        public ICommand SaveAsConfigCommand { get; set; }

        public HardwareConfigurationViewModel()
        {
            _midiGateway = new MidiGateway(MessengerInstance);
            _serialGateway = new SerialGateway(MessengerInstance);

            OutputMidiDevices = new ObservableCollection<OutputMidiDevice>();
            InputComPorts = new ObservableCollection<InputCOMPort>();
            SupportedBaudRates = new ObservableCollection<int>();

            OnConnectComCommand = new RelayCommand(() => _serialGateway.Connect(), () => this.SelectedComPort?.Id >= 0);
            OnConnectMidiCommand = new RelayCommand(() => _midiGateway.Connect(), () => this.SelectedOutputMidiDevice?.Id >= 0);
            OnRefreshComCommand = new RelayCommand(OnRefreshCOMDevices, () => !this.IsComConnected);
            OnRefreshMidiCommand = new RelayCommand(OnRefreshMidiDevices, () => !this.IsMidiConnected);
            OnSendMidiTestCommand = new RelayCommand(() => _midiGateway.SendTest(), () => IsMidiConnected);
            
            LoadConfigCommand = new RelayCommand(OnLoadConfig);
            SaveConfigCommand = new RelayCommand(OnSaveConfig);
            SaveAsConfigCommand = new RelayCommand(OnSaveAsConfig);

            OnRefreshMidiDevices();
            OnRefreshCOMDevices();

            MessengerInstance.Register<MidiConnectionStateChangedMessage>(this, (o) => this.IsMidiConnected = o.NewState == MidiConnectionStatus.Connected);
            MessengerInstance.Register<SerialConnectionStateChangedMessage>(this, (o) => this.IsComConnected = o.NewState);
        }
        
        public HardwareConfigurationViewModel(HardwareConfigurationData config, string path) : this()
        {
            this.SelectedComPort = config.ComPort;
            this.SelectedOutputMidiDevice = config.MidiOut;
            this.SelectedSpeed = config.Speed;
            this.Path = path;
        }

        private void OnSaveAsConfig()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Moonburst config|*.mbconfig";
            saveFileDialog1.Title = "Save layout";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                Path = saveFileDialog1.FileName;
                DataSerializer<HardwareConfigurationViewModel, HardwareConfigurationData>.SaveToFile(this);
            }
        }

        private void OnSaveConfig()
        {
            DataSerializer<HardwareConfigurationViewModel, HardwareConfigurationData>.SaveToFile(this);
        }

        private void OnLoadConfig()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Moonburst config|*.mbconfig";
            openFileDialog1.Title = "Load layout";
            if (openFileDialog1.ShowDialog() == true)
            {
                Load(DataSerializer<HardwareConfigurationViewModel, HardwareConfigurationData>.LoadFromFile(openFileDialog1.FileName));
            }
        }

        void OnRefreshMidiDevices()
        {
            OutputMidiDevices.Clear();
            _midiGateway.GetDevices().ForEach(OutputMidiDevices.Add);
            RaisePropertyChanged("SelectedOutput");
        }

        void OnRefreshCOMDevices()
        {
            InputComPorts.Clear();
            SupportedBaudRates.Clear();
            _serialGateway.GetPorts().ForEach(InputComPorts.Add);
            _serialGateway.GetRates().ForEach(SupportedBaudRates.Add);
            RaisePropertyChanged("SelectedComPort");
        }

        public void Load(HardwareConfigurationViewModel config)
        {
            this.SelectedComPort = config.SelectedComPort;
            this.SelectedOutputMidiDevice = config.SelectedOutputMidiDevice;
            this.SelectedSpeed = config.SelectedSpeed;
            this.Path = config.Path;
        }

        public string Path { get; set; }
        public string Default { get => "default_hardware.xml"; }

        HardwareConfigurationData IFileSerializableType<HardwareConfigurationData>.GetData()
        {
            return new HardwareConfigurationData()
            {
                ComPort = this.SelectedComPort,
                MidiOut = this.SelectedOutputMidiDevice,
                Speed = this.SelectedSpeed
            };
        }

        public IFileSerializableType<HardwareConfigurationData> CreateFromData(HardwareConfigurationData data, string path)
        {
            return new HardwareConfigurationViewModel(data, path);
        }

        public class HardwareConfigurationData : IFileSerializableData
        {
            public InputCOMPort ComPort { get; set; }
            public OutputMidiDevice MidiOut { get; set; }
            public int Speed { get; set; }
        }
    }
}
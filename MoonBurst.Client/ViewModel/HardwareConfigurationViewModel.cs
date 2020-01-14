using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using MoonBurst.Core;
using MoonBurst.Model;
using MoonBurst.Model.Messages;

namespace MoonBurst.ViewModel
{
    public class HardwareConfigurationViewModel : ViewModelBase
    {
        private IMidiGateway _midiGateway;
        private ISerialGateway _serialGateway;
        private IArduinoGateway _arduinoConfig;
        private IMessenger _messenger;

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
                RefreshAvailabledSpeeds();
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
        public ObservableCollection<ArduinoConfigPortViewModel> ArduinoPorts { get; }

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

        public HardwareConfigurationViewModel(IMidiGateway midiGateway, ISerialGateway serialGateway, IArduinoGateway arduinoGateway, IMessenger messenger)
        {
            _midiGateway = midiGateway;
            _serialGateway = serialGateway;
            _arduinoConfig = arduinoGateway;
            _messenger = messenger;

            ArduinoPorts = new ObservableCollection<ArduinoConfigPortViewModel>();
            OutputMidiDevices = new ObservableCollection<OutputMidiDevice>();
            InputComPorts = new ObservableCollection<InputCOMPort>();
            SupportedBaudRates = new ObservableCollection<int>();

            OnConnectComCommand = new RelayCommand(() => _serialGateway.Connect(), () => !String.IsNullOrEmpty(this.SelectedComPort?.Id));
            OnConnectMidiCommand = new RelayCommand(() => _midiGateway.Connect(), () => this.SelectedOutputMidiDevice?.Id >= 0);
            OnRefreshComCommand = new RelayCommand(OnRefreshCOMDevices, () => !this.IsComConnected);
            OnRefreshMidiCommand = new RelayCommand(OnRefreshMidiDevices, () => !this.IsMidiConnected);
            OnSendMidiTestCommand = new RelayCommand(() => _midiGateway.SendTest(), () => IsMidiConnected);
            
            LoadConfigCommand = new RelayCommand(OnLoadConfig);
            SaveConfigCommand = new RelayCommand(OnSaveConfig);
            SaveAsConfigCommand = new RelayCommand(OnSaveAsConfig);

            OnRefreshMidiDevices();
            OnRefreshCOMDevices();
            OnRefreshArduinoPorts();

            _messenger.Register<MidiConnectionStateChangedMessage>(this, (o) => this.IsMidiConnected = o.NewState == MidiConnectionStatus.Connected);
            _messenger.Register<SerialConnectionStateChangedMessage>(this, OnSerialStateChange);
        }

        private void OnSerialStateChange(SerialConnectionStateChangedMessage obj)
        {
            this.IsComConnected = obj.NewState;
            if(!IsComConnected)
                this.OnRefreshCOMDevices();
        }

        public void Close()
        {
            _serialGateway.Close();
            _midiGateway.Close();
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
                DataSerializer<HardwareConfigurationData>.SaveToFile(GetData());
            }
        }

        private void OnSaveConfig()
        {
            DataSerializer<HardwareConfigurationData>.SaveToFile(GetData());
        }

        private void OnLoadConfig()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Moonburst config|*.mbconfig";
            openFileDialog1.Title = "Load layout";
            if (openFileDialog1.ShowDialog() == true)
            {
                LoadData(DataSerializer<HardwareConfigurationData>.LoadFromFile(openFileDialog1.FileName));
            }
        }

        void OnRefreshMidiDevices()
        {
            OutputMidiDevices.Clear();
            _midiGateway.GetDevices().ForEach(OutputMidiDevices.Add);
            RaisePropertyChanged("SelectedOutput");
        }

        void RefreshAvailabledSpeeds()
        {
            if (this.SelectedComPort != null)
            {
                SupportedBaudRates.Clear();
                var selectedSpeed = SelectedSpeed;
                _serialGateway.GetRates().Where(r => r <= this.SelectedComPort.MaxBaudRate).ToList()
                    .ForEach(SupportedBaudRates.Add);
                SelectedSpeed = selectedSpeed;
            }
        }

        void OnRefreshCOMDevices()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var selectedCom = SelectedComPort?.Name;
                InputComPorts.Clear();
                _serialGateway.GetPorts().ForEach(InputComPorts.Add);

                if (!String.IsNullOrEmpty(selectedCom) && InputComPorts.Any(o => o.Name == selectedCom))
                    SelectedComPort = InputComPorts.FirstOrDefault(o => o.Name == selectedCom);
                else
                    SelectedComPort = null;

                RaisePropertyChanged("SelectedComPort");
            });
        }

        void OnRefreshArduinoPorts()
        {
            ArduinoPorts.Clear();
            foreach (var port in _arduinoConfig.Ports.OrderBy(c => c.Position))
                ArduinoPorts.Add(new ArduinoConfigPortViewModel(port, _messenger));
        }
        
        public string Path { get; set; }

        public void UpdateArduinoPorts(List<ArduinoPortConfig> data)
        {
            foreach (var port in ArduinoPorts)
            {
                var dataPort = data.FirstOrDefault(d => d.Position == port.Position);
                if (dataPort != null)
                {
                    port.IsEnabled = dataPort.IsEnabled;
                    port.Connect(dataPort.ConnectedDevice);
                }
            }
        }
        
        public void LoadData(HardwareConfigurationData config)
        {
            this.SelectedComPort = config.ComPort;
            this.SelectedOutputMidiDevice = config.MidiOut;
            this.SelectedSpeed = config.Speed;
            this.Path = config.Path;
            this.UpdateArduinoPorts(config.ArduinoPorts);
        }

        HardwareConfigurationData GetData()
        {
            return new HardwareConfigurationData()
            {
                ComPort = this.SelectedComPort,
                MidiOut = this.SelectedOutputMidiDevice,
                Speed = this.SelectedSpeed,
                ArduinoPorts = ArduinoPorts.ToList().ConvertAll(c => c.GetData()).ToList(),
                Path = this.Path
            };
        }

        public class HardwareConfigurationData : IFileSerializableType
        {
            [XmlIgnore]
            public string Path { get; set; }
            [XmlIgnore]
            public string Default { get => "default_hardware.xml"; }

            public InputCOMPort ComPort { get; set; }
            public OutputMidiDevice MidiOut { get; set; }
            public List<ArduinoPortConfig> ArduinoPorts { get; set; }
            public int Speed { get; set; }
        }
    }
}
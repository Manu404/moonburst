using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using MoonBurst.Api.Enums;
using MoonBurst.Api.Services;
using MoonBurst.Core.Serializer;
using MoonBurst.Model;
using MoonBurst.Model.Messages;
using MoonBurst.Model.Serializable;
using MoonBurst.ViewModel.Interfaces;

namespace MoonBurst.ViewModel
{
    public class HardwareConfigurationViewModel : ViewModelBase, IHardwareConfigurationViewModel
    {
        private IMidiGateway _midiGateway;
        private ISerialGateway _serialGateway;
        private IArduinoGateway _arduinoConfig;
        private IMessenger _messenger;
        private IClientConfigurationViewModel _config;
        private ISerializer<IHardwareConfigurationViewModel> _serializer;

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

        public InputComPortData SelectedComPort
        {
            get => _serialGateway.CurrentPort;
            set
            {
                _serialGateway.CurrentPort = value;
                RaisePropertyChanged();
                RefreshAvailabledSpeeds();
            }
        }

        public OutputMidiDeviceData SelectedOutputMidiDevice
        {
            get => _midiGateway.SelectedOutput;
            set
            {
                _midiGateway.SelectedOutput = value;
                RaisePropertyChanged();
            }
        }

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

        public ObservableCollection<OutputMidiDeviceData> OutputMidiDevices { get; set; }
        public ObservableCollection<InputComPortData> InputComPorts { get; set; }
        public ObservableCollection<int> SupportedBaudRates { get; set; }
        public ObservableCollection<ArduinoConfigPortViewModel> ArduinoPorts { get; }

        public string CurrentPath { get; set; }

        public ICommand OnSendMidiTestCommand { get; set; }
        public ICommand OnRefreshMidiCommand { get; set; }
        public ICommand OnRefreshComCommand { get; set; }
        public ICommand OnConnectMidiCommand { get; set; }
        public ICommand OnConnectComCommand { get; set; }

        public ICommand LoadConfigCommand { get; set; }
        public ICommand SaveConfigCommand { get; set; }
        public ICommand SaveAsConfigCommand { get; set; }

        public HardwareConfigurationViewModel(IMidiGateway midiGateway, 
            ISerialGateway serialGateway, 
            IArduinoGateway arduinoGateway, 
            IMessenger messenger, 
            IClientConfigurationViewModel config,
            ISerializer<IHardwareConfigurationViewModel> serializer)
        {
            _midiGateway = midiGateway;
            _serialGateway = serialGateway;
            _arduinoConfig = arduinoGateway;
            _messenger = messenger;
            _config = config;
            _serializer = serializer;

            ArduinoPorts = new ObservableCollection<ArduinoConfigPortViewModel>();
            OutputMidiDevices = new ObservableCollection<OutputMidiDeviceData>();
            InputComPorts = new ObservableCollection<InputComPortData>();
            SupportedBaudRates = new ObservableCollection<int>();

            OnConnectComCommand = new RelayCommand(() => _serialGateway.Connect(_arduinoConfig.Ports), () => !String.IsNullOrEmpty(this.SelectedComPort?.Id));
            OnConnectMidiCommand = new RelayCommand(() => _midiGateway.Connect(), () => this.SelectedOutputMidiDevice?.Id >= 0);
            OnRefreshComCommand = new RelayCommand(OnRefreshCOMDevices, () => !this.IsComConnected);
            OnRefreshMidiCommand = new RelayCommand(OnRefreshMidiDevices, () => !this.IsMidiConnected);
            OnSendMidiTestCommand = new RelayCommand(() => _midiGateway.SendTest(), () => IsMidiConnected);
            
            LoadConfigCommand = new RelayCommand(OnLoadConfig);
            SaveConfigCommand = new RelayCommand(SaveLastConfig);
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
            SaveConfigCommand.Execute(null);
            _serialGateway.Close();
            _midiGateway.Close();
        }
        
        void OnRefreshMidiDevices()
        {
            OutputMidiDevices.Clear();
            _midiGateway.GetDevices().ForEach(OutputMidiDevices.Add);
            RaisePropertyChanged();
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
            if (Application.Current.Dispatcher != null)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var selectedCom = SelectedComPort?.Name;
                    InputComPorts.Clear();
                    _serialGateway.GetPorts().ToList().ForEach(InputComPorts.Add);

                    if (!String.IsNullOrEmpty(selectedCom) && InputComPorts.Any(o => o.Name == selectedCom))
                        SelectedComPort = InputComPorts.FirstOrDefault(o => o.Name == selectedCom);
                    else
                        SelectedComPort = null;

                    RaisePropertyChanged();
                });
        }

        void OnRefreshArduinoPorts()
        {
            ArduinoPorts.Clear();
            foreach (var port in _arduinoConfig.Ports.OrderBy(c => c.Position))
                ArduinoPorts.Add(new ArduinoConfigPortViewModel(port, _messenger));
        }


        public void UpdateArduinoPorts(List<ArduinoPortConfigData> data)
        {
            if (data == null) return;
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

        private void OnSaveAsConfig()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Moonburst config|*.mbconfig";
            saveFileDialog1.Title = "Save layout";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                _serializer.Save(this, saveFileDialog1.FileName);
            }
        }

        private void OnLoadConfig()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Moonburst config|*.mbconfig";
            openFileDialog1.Title = "Load layout";
            if (openFileDialog1.ShowDialog() == true)
            {
                _serializer.Load(openFileDialog1.FileName, this);
            }
        }

        public void Save(string path)
        {
            _serializer.Save(this, path);
            _config.LastHardwareConfigurationPath = path;
        }

        public void Load(string path, IHardwareConfigurationViewModel source)
        {
            _serializer.Load(path, this);
            _config.LastHardwareConfigurationPath = path;
        }

        public void LoadLastConfig()
        {
            Load(_config.LastHardwareConfigurationPath, this);
        }

        public void SaveLastConfig()
        {
            Save(_config.LastHardwareConfigurationPath);
        }
    }
}
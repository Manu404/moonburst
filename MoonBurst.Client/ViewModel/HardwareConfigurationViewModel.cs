using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using MoonBurst.Api.Client;
using MoonBurst.Api.Enums;
using MoonBurst.Api.Gateway.Arduino;
using MoonBurst.Api.Gateway.Midi;
using MoonBurst.Api.Gateway.Serial;
using MoonBurst.Api.Serializer;
using MoonBurst.Core.Helper;
using MoonBurst.Model;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{ 
    public class FileDialogProvider : IFileDialogProvider
    {
        public string ShowSaveDialog(string title, string filter)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog {Filter = filter, Title = title};
            saveFileDialog1.ShowDialog();
            return saveFileDialog1.FileName;
        }

        public string ShowLoadDialog(string title, string filter)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog {Filter = filter, Title = title};
            openFileDialog1.ShowDialog();
            return openFileDialog1.FileName;
        }
    }

    public class HardwareConfigurationViewModel : ViewModelBase, IHardwareConfigurationViewModel
    {
        private int lastSpeed { get; set; }
        private readonly IMidiGateway _midiGateway;
        private readonly ISerialGateway _serialGateway;
        private readonly IArduinoGateway _arduinoConfig;
        private readonly IApplicationConfigurationViewModel _config;
        private readonly ISerializer<IHardwareConfigurationViewModel> _serializer;
        private readonly IFactory<IArduinoConfigPortViewModel, IArduinoPort> _arduinoPortFactory;
        private readonly IFileDialogProvider _dialogProvider;

        public bool IsComConnected { get; set; }
        public ComPort SelectedComPort
        {
            get => _serialGateway.CurrentPort;
            set
            {
                _serialGateway.CurrentPort = value;
                RaisePropertyChanged();
                RefreshAvailabledSpeeds();
            }
        }

        public ComPortSpeed SelectedSpeed
        {
            get => _serialGateway.CurrentSpeed;
            set
            {
                _serialGateway.CurrentSpeed = value;
                if (value?.BaudRate != 0) lastSpeed = value.BaudRate;
                RaisePropertyChanged();
            }
        }

        public bool IsMidiConnected { get; set; }
        public MidiDevice SelectedOutputMidiDevice
        {
            get => _midiGateway.SelectedOutput;
            set
            {
                _midiGateway.SelectedOutput = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MidiDevice> OutputMidiDevices { get; set; }
        public ObservableCollection<ComPort> InputComPorts { get; set; }
        public ObservableCollection<ComPortSpeed> SupportedBaudRates { get; set; }
        public ObservableCollection<IArduinoConfigPortViewModel> ArduinoPorts { get; }

        public string CurrentPath { get; set; }

        public ICommand OnSendMidiTestCommand { get; set; }
        public ICommand OnRefreshMidiCommand { get; set; }
        public ICommand OnRefreshComCommand { get; set; }
        public ICommand OnConnectMidiCommand { get; set; }
        public ICommand OnDisconnectMidiCommand { get; set; }
        public ICommand OnConnectComCommand { get; set; }
        public ICommand OnDisconnectComCommand { get; set; }

        public ICommand LoadConfigCommand { get; set; }
        public ICommand SaveConfigCommand { get; set; }
        public ICommand SaveAsConfigCommand { get; set; }

        public HardwareConfigurationViewModel(IMidiGateway midiGateway, 
            ISerialGateway serialGateway, 
            IArduinoGateway arduinoGateway, 
            IApplicationConfigurationViewModel config,
            ISerializer<IHardwareConfigurationViewModel> serializer,
            IFactory<IArduinoConfigPortViewModel, IArduinoPort> arduinoPortFactory)
        {
            _midiGateway = midiGateway;
            _serialGateway = serialGateway;
            _arduinoConfig = arduinoGateway;
            _config = config;
            _serializer = serializer;
            _arduinoPortFactory = arduinoPortFactory;

            _dialogProvider = new FileDialogProvider();

            ArduinoPorts = new ObservableCollection<IArduinoConfigPortViewModel>();
            OutputMidiDevices = new ObservableCollection<MidiDevice>();
            InputComPorts = new ObservableCollection<ComPort>();
            SupportedBaudRates = new ObservableCollection<ComPortSpeed>();

            OnConnectComCommand = new RelayCommand(() => _serialGateway.Connect(_arduinoConfig.Ports), () => !String.IsNullOrEmpty(this.SelectedComPort?.Id));
            OnDisconnectComCommand = new RelayCommand(() => _serialGateway.Close(), () => !String.IsNullOrEmpty(this.SelectedComPort?.Id) && IsComConnected);
            OnConnectMidiCommand = new RelayCommand(() => _midiGateway.Connect(), () => this.SelectedOutputMidiDevice?.Id >= 0);
            OnDisconnectMidiCommand = new RelayCommand(() => _midiGateway.Close(), () => this.SelectedOutputMidiDevice?.Id >= 0 && IsMidiConnected);
            OnRefreshComCommand = new RelayCommand(OnRefreshCOMDevices, () => !this.IsComConnected);
            OnRefreshMidiCommand = new RelayCommand(OnRefreshMidiDevices, () => !this.IsMidiConnected);
            OnSendMidiTestCommand = new RelayCommand(() => _midiGateway.SendTest(), () => IsMidiConnected);
            
            LoadConfigCommand = new RelayCommand(OnLoad);
            SaveConfigCommand = new RelayCommand(OnSaveLast);
            SaveAsConfigCommand = new RelayCommand(OnSaveAs);

            OnRefreshMidiDevices();
            OnRefreshCOMDevices();
            OnRefreshArduinoPorts();

            midiGateway.ConnectionStateChanged += (sender, args) => this.IsMidiConnected = args.NewState == MidiConnectionState.Connected;
            serialGateway.ConnectionStateChanged += (sender, args) => OnSerialStateChanged(args);
            PropertyChanged += (sender, args) => this.ConfigurationChanged?.Invoke(this, args);
        }

        private void OnSerialStateChanged(SerialConnectionStateChangedEventArgs obj)
        {
            IsComConnected = obj.NewState;
            if(!IsComConnected)
                OnRefreshCOMDevices();
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
            SupportedBaudRates.Clear();

            if (SelectedComPort != null)
            {
                _serialGateway.GetSupportedSpeeds().Where(r => r.BaudRate <= SelectedComPort.MaxBaudRate).ToList()
                    .ForEach(SupportedBaudRates.Add);
            }

            if (SupportedBaudRates.Any(s => s.BaudRate == lastSpeed))
                SelectedSpeed = SupportedBaudRates.First(s => s.BaudRate == lastSpeed);
        }

        void OnRefreshCOMDevices()
        {
            if (Application.Current?.Dispatcher != null)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    UpdateCOMDevices(SelectedComPort?.Name);
                });
            else UpdateCOMDevices(SelectedComPort?.Name);
        }

        public void UpdateCOMDevices(string selectedCom)
        {
            InputComPorts.Clear();
            _serialGateway.GetPorts().ToList().ForEach(InputComPorts.Add);

            if (!String.IsNullOrEmpty(selectedCom) && InputComPorts.Any(o => o.Name == selectedCom))
                SelectedComPort = InputComPorts.FirstOrDefault(o => o?.Name == selectedCom);
            else
                SelectedComPort = null;

            RaisePropertyChanged();
        }

        void OnRefreshArduinoPorts()
        {
            ArduinoPorts.Clear();
            foreach (var port in _arduinoConfig.Ports.OrderBy(c => c.Position))
                ArduinoPorts.Add(_arduinoPortFactory.Build(port));
        }


        public void UpdateArduinoPorts(List<ArduinoPortConfigModel> data)
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

        public string GetStatusString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_midiGateway.GetStatusString());
            sb.Append(" | ");
            sb.Append(_serialGateway.GetStatusString());
            return sb.ToString();
        }

        public event EventHandler ConfigurationChanged;

        private void OnSaveAs()
        {
            Save(_dialogProvider.ShowSaveDialog("Save config", FileAssociationsHelper.ConfigFilter));
        }

        private void OnLoad()
        {
            Load(_dialogProvider.ShowLoadDialog("Load config", FileAssociationsHelper.ConfigFilter));
        }

        public void Save(string path)
        {
            _serializer.Save(this, path);
            _config.LastHardwareConfigurationPath = path;
        }

        public void Load(string path)
        {
            _serializer.Load(path, this);
            _config.LastHardwareConfigurationPath = path;
        }

        public void LoadLast()
        {
            Load(_config.LastHardwareConfigurationPath);
        }

        public void OnSaveLast()
        {
            Save(_config.LastHardwareConfigurationPath);
        }
    }
}
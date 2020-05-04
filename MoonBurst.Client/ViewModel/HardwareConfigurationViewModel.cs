using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using MoonBurst.Api.Enums;
using MoonBurst.Api.Gateway.Arduino;
using MoonBurst.Api.Gateway.Midi;
using MoonBurst.Api.Gateway.Serial;
using MoonBurst.Api.Hardware;
using MoonBurst.Api.Serializer;
using MoonBurst.Core;
using MoonBurst.Core.Helper;
using MoonBurst.Core.Serializer;
using MoonBurst.Helper;
using MoonBurst.Model;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.ViewModel
{ 
    public class LoadSaveDialogProvider : ILoadSaveDialogProvider
    {
        public string ShowSaveDialog(string title, string filter)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = filter;
            saveFileDialog1.Title = title;
            saveFileDialog1.ShowDialog();
            return saveFileDialog1.FileName;
        }

        public string ShowLoadDialog(string title, string filter)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = filter;
            openFileDialog1.Title = title;
            return openFileDialog1.FileName;
        }
    }

    public class HardwareConfigurationViewModel : ViewModelBase, IHardwareConfigurationViewModel
    {
        private readonly IMidiGateway _midiGateway;
        private readonly ISerialGateway _serialGateway;
        private readonly IArduinoGateway _arduinoConfig;
        private readonly IApplicationConfigurationViewModel _config;
        private readonly ISerializer<IHardwareConfigurationViewModel> _serializer;
        private readonly IFactory<IArduinoConfigPortViewModel, IArduinoPort> _arduinoPortFactory;
        private readonly ILoadSaveDialogProvider _dialogProvider;

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

        public MidiDevice SelectedOutputMidiDevice
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

        public ObservableCollection<MidiDevice> OutputMidiDevices { get; set; }
        public ObservableCollection<ComPort> InputComPorts { get; set; }
        public ObservableCollection<int> SupportedBaudRates { get; set; }
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

            _dialogProvider = new LoadSaveDialogProvider();

            ArduinoPorts = new ObservableCollection<IArduinoConfigPortViewModel>();
            OutputMidiDevices = new ObservableCollection<MidiDevice>();
            InputComPorts = new ObservableCollection<ComPort>();
            SupportedBaudRates = new ObservableCollection<int>();

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
        }

        private void OnSerialStateChanged(SerialConnectionStateChangedEventArgs obj)
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
            SupportedBaudRates.Clear();
            if (this.SelectedComPort != null)
            {
                var selectedSpeed = SelectedSpeed;

                _serialGateway.GetRates().Where(r => r <= this.SelectedComPort.MaxBaudRate).ToList()
                    .ForEach(SupportedBaudRates.Add);

                if(this.SupportedBaudRates.Contains(selectedSpeed))
                    SelectedSpeed = selectedSpeed;
            }
        }

        void OnRefreshCOMDevices()
        {
            if (Application.Current?.Dispatcher != null)
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

        private void OnSaveAs()
        {
            Save(_dialogProvider.ShowSaveDialog("Save config", FileAssociationsHelper.ConfigFilter));
        }

        private void OnLoad()
        {
            Load(_dialogProvider.ShowSaveDialog("Load config", FileAssociationsHelper.ConfigFilter));
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
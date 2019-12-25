using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using MoonBurst.Model;
using Sanford.Multimedia.Midi;

namespace MoonBurst.ViewModel
{
    public class HardwareConfigurationViewModel : ViewModelBase, IFileSerializableType<HardwareConfigurationViewModel.HardwareConfigurationData>
    {
        private bool _isMidiConnected;
        private bool _isComConnected;

        private InputCOMPort _comPort;
        private OutputMidiDevice _midiOut;
        private int MessageLength;
        private int _speed;
        
        public int SelectedSpeed
        {
            get => _speed;
            set
            {
                _speed = value;
                RaisePropertyChanged();
            }
        }

        public InputCOMPort SelectedComPort
        {
            get => _comPort;
            set
            {
                _comPort = value;
                RaisePropertyChanged();
            }
        }

        public OutputMidiDevice SelectedOutputMidiDevice
        {
            get => _midiOut;
            set
            {
                _midiOut = value;
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

        public ICommand SendMidiTestCommand { get; set; }
        public ICommand OnRefreshMidiCommand { get; set; }
        public ICommand OnRefreshComCommand { get; set; }
        public ICommand OnConnectMidiCommand { get; set; }
        public ICommand OnConnectComCommand { get; set; }

        public ICommand LoadConfigCommand { get; set; }
        public ICommand SaveConfigCommand { get; set; }
        public ICommand SaveAsConfigCommand { get; set; }

        public HardwareConfigurationViewModel()
        {
            
            OutputMidiDevices = new ObservableCollection<OutputMidiDevice>();
            InputComPorts = new ObservableCollection<InputCOMPort>();
            SupportedBaudRates = new ObservableCollection<int>
            {
                300,
                600,
                1200,
                2400,
                4800,
                9600,
                19200,
                38400,
                57600,
                115200,
                230400,
                460800,
                921600
            };

            OnConnectComCommand = new RelayCommand(OnConnectCom, () => this.SelectedComPort?.Id >= 0);
            OnConnectMidiCommand = new RelayCommand(OnConnectMidi, () => this.SelectedOutputMidiDevice?.Id >= 0);
            OnRefreshComCommand = new RelayCommand(OnRefreshCOMDevices, () => !this.IsComConnected);
            OnRefreshMidiCommand = new RelayCommand(OnRefreshMidiDevices, () => !this.IsMidiConnected);
            //SendMidiTestCommand = new RelayCommand(OnSendMidiTest, () => IsMidiConnected);
            
            LoadConfigCommand = new RelayCommand(OnLoadConfig);
            SaveConfigCommand = new RelayCommand(OnSaveConfig);
            SaveAsConfigCommand = new RelayCommand(OnSaveAsConfig);

            OnRefreshMidiDevices();
            OnRefreshCOMDevices();
        }

        public HardwareConfigurationViewModel(HardwareConfigurationData config, string path) : this()
        {
            this.SelectedComPort = config.ComPort;
            this.SelectedOutputMidiDevice = config.MidiOut;
            this.MessageLength = config.MessageLength;
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

        void OnConnectMidi()
        {
            //if (!this.IsMidiConnected && this.SelectedOutputMidiDevice?.Id >= 0)
            //{
            //    _outDevice = new OutputDevice(this.SelectedOutputMidiDevice.Id);
            //    this.IsMidiConnected = true;
            //}
            //else
            //{
            //    _outDevice?.Close();
            //    this.IsMidiConnected = false;
            //}
        }

        void OnConnectCom()
        {
            //if (!this.IsComConnected)
            //{
            //    try
            //    {
            //        if (!Int32.TryParse(this.SelectedSpeed, out var speed))
            //        {
            //            speed = 9600;
            //            this.SelectedSpeed = "9600";
            //        }
            //        CurrentSerialPort = new SerialPort(this.SelectedComPort.Name, speed, Parity.None, 8, StopBits.One);
            //        CurrentSerialPort.DataReceived += OnSerialData;
            //        CurrentSerialPort.Open();
            //        WriteLine($"Port {this.SelectedComPort.Name} opened ({speed}bauds)");
            //        this.IsComConnected = true;
            //    }
            //    catch (Exception e)
            //    {
            //        WriteLine(e.Message);
            //        this.IsComConnected = false;
            //    }
            //}
            //else
            //{
            //    try
            //    {
            //        if (CurrentSerialPort != null && CurrentSerialPort.IsOpen)
            //        {
            //            CurrentSerialPort.DataReceived -= OnSerialData;
            //            CurrentSerialPort.Close();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        WriteLine(e.Message);
            //    }
            //    this.IsComConnected = false;
            //}
        }

        void OnRefreshMidiDevices()
        {
            this.RefreshMidiDevices();
        }

        void OnRefreshCOMDevices()
        {
            this.RefreshCOMDevices();
        }


        public void RefreshMidiDevices()
        {
            OutputMidiDevices.Clear();
            if (OutputDevice.DeviceCount > 0)
            {
                //WriteLine($"{OutputDevice.DeviceCount} devices found:");
                for (int i = 0; i < OutputDevice.DeviceCount; i++)
                {
                    string deviceName = OutputDevice.GetDeviceCapabilities(i).name;
                    //WriteLine(deviceName);
                    OutputMidiDevices.Add(new OutputMidiDevice() { Name = deviceName, Id = i });
                }
            }
            else
            {
                //WriteLine("No devices found... :(");
                OutputMidiDevices.Add(new OutputMidiDevice() { Name = "No device output devices available...", Id = -1 });
            }
            RaisePropertyChanged("SelectedOutputMidiDevice");
        }

        public void RefreshCOMDevices()
        {
            InputComPorts.Clear();
            string[] ports = SerialPort.GetPortNames();
            if (ports.Any())
            {
                int i = 0;
                //WriteLine($"{ports.Count()} COM ports found:");
                foreach (var port in ports)
                {
                    //WriteLine(port);
                    InputComPorts.Add(new InputCOMPort() { Name = port, Id = i });
                    i++;
                }
            }
            else
            {
                //WriteLine("No devices found... :(");
                InputComPorts.Add(new InputCOMPort() { Name = "No COM ports available...", Id = -1 });
            }
            RaisePropertyChanged("SelectedComPort");
        }

        public void Load(HardwareConfigurationViewModel config)
        {
            this.SelectedComPort = config.SelectedComPort;
            this.SelectedOutputMidiDevice = config.SelectedOutputMidiDevice;
            this.MessageLength = config.MessageLength;
            this.SelectedSpeed = config.SelectedSpeed;
            this.Path = config.Path;
        }

        public string Path { get; set; }
        public string Default { get => "default_hardware.xml"; }

        HardwareConfigurationData IFileSerializableType<HardwareConfigurationData>.GetData()
        {
            return new HardwareConfigurationData()
            {
                ComPort = this._comPort,
                MidiOut = this._midiOut,
                MessageLength = this.MessageLength,
                Speed = this._speed
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
            public int MessageLength { get; set; }
            public int Speed { get; set; }
        }
    }
}
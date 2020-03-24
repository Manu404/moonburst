using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading;
using MoonBurst.Api.Hardware;
using MoonBurst.Api.Parser;
using MoonBurst.Api.Services;
using MoonBurst.Core.Hardware.Parser;

namespace MoonBurst.Core.Hardware
{
    public class SerialGateway : ISerialGateway, IHardwareService
    {
        private IControllerParser[] _footswitchParsers;
        private IArduinoPort[] _arduinoPorts;
        private SerialPort _serialPort;
        private bool _isConnected;
        private Timer _checkStatusTimer;

        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                if (value != _isConnected)
                {
                    _isConnected = value;
                   // _messenger.Send(new SerialConnectionStateChangedMessage(){ NewState = value });
                }
            }
        }

        public int CurrentSpeed { get; set; }
        public InputComPortData CurrentPort { get; set; }

        public SerialGateway()
        {
            _checkStatusTimer = new Timer(OnCheckStatus, this, 0, 1000);
        }

        private void InitializeParsers(IArduinoPort[] ports)
        {
            _arduinoPorts = ports;
            _footswitchParsers = new IControllerParser[ports.Length];
            for (var i = 0; i < ports.Length; i++)
            {
                if (ports[i].ConnectedDevice != null)
                {
                    _footswitchParsers[i] = new Fs3XParser();
                }
            }
        }

        private void OnCheckStatus(object state)
        {
            if (!IsConnected) return;
            if (!_serialPort.IsOpen)
                Close();
        }

        public void Connect(IArduinoPort[] ports)
        {
            if (!IsConnected)
            {
                try
                {
                    _serialPort = new SerialPort(CurrentPort.Id, CurrentSpeed, Parity.None, 8, StopBits.One);
                    _serialPort.DataReceived += SerialPortOnDataReceived;
                    _serialPort.Open();
                    InitializeParsers(ports);
                    IsConnected = true;
                }
                catch (Exception)
                {
                    IsConnected = false;
                }
            }
            else
            {
                try
                {
                    if (_serialPort != null && _serialPort.IsOpen)
                    {
                        _serialPort.DataReceived -= SerialPortOnDataReceived;
                        _serialPort.Close();
                    }
                }
                catch (Exception)
                {
                    //TODO: loging
                }
                IsConnected = false;
            }
        }

        public void Close()
        {
            if (!IsConnected) return;
            _serialPort.DataReceived -= SerialPortOnDataReceived;
            _serialPort.Close();
            _serialPort.Dispose();
            IsConnected = false;
        }

        private string accumulator = "";
        Regex group = new Regex(@"\?([0-9]+);([0-9]+);([0-9]+)\!", RegexOptions.Compiled);
        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            accumulator += _serialPort.ReadExisting();
            if (accumulator.Length < 100 || _arduinoPorts == null) return;

            var lines = accumulator.Split('\n');
            accumulator = "";

            foreach (var line in lines)
            {
                try
                {
                    Match match = group.Match(line);
                    if (match.Groups.Count == 4)
                    {
                        byte pind = Byte.Parse(match.Groups[1].Value);
                        byte pinb = Byte.Parse(match.Groups[2].Value);

                        int digitalPins = (pind | (pinb << 8));
                        for (var pos = 0; pos < _arduinoPorts.Length; pos++)
                        {
                            digitalPins = digitalPins >> 2;
                            //int current = digitalPins & 3;

                            //if (_footswitchParsers[pos] != null)
                            //    _messenger.Send(new ControllerStateMessage { States = _footswitchParsers[pos].ParseState(current, pos), Port = pos });
                        }

                    }
                }
                catch (Exception)
                {
                    //TODO: loging
                }
            }
        }

        public IEnumerable<InputComPortData> GetPorts()
        {
            var result = new List<InputComPortData>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort");
                foreach (var o in searcher.Get())
                {
                    var queryObj = (ManagementObject) o;
                    try
                    {
                        result.Add(new InputComPortData(queryObj["DeviceID"].ToString())
                        {
                            Name = queryObj["Name"].ToString(),
                            MaxBaudRate = Int32.Parse(queryObj["MaxBaudRate"].ToString())
                        });
                    }
                    catch
                    {
                        //TODO: loging
                    }
                }
                if(!result.Any())
                    result.Add(new InputComPortData("") { Name = "No COM ports available...", MaxBaudRate = 0});

            }
            catch (ManagementException)
            {
                //MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
            return result;
        }

        public IEnumerable<int> GetRates()
        {
            return new List<int>
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
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Threading;
using System.Windows;
using Castle.Core.Internal;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Core.Parser;
using MoonBurst.Model;
using MoonBurst.Model.Messages;
using MoonBurst.Model.Parser;

namespace MoonBurst.Core
{
    public interface ISerialGateway
    {
        bool IsConnected { get; }
        int CurrentSpeed { get; set; }
        InputCOMPort CurrentPort { get; set; }
        List<InputCOMPort> GetPorts();
        List<int> GetRates();
        void Connect();
        void Close();
    }

    public class SerialGateway : ISerialGateway
    {
        private IControllerParser _footswitchParser;
        private IMessenger _messenger;
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
                    _messenger.Send(new SerialConnectionStateChangedMessage(){ NewState = value });
                }
            }
        }

        public int CurrentSpeed { get; set; }
        public InputCOMPort CurrentPort { get; set; }

        public SerialGateway(IMessenger messenger)
        {
            _messenger = messenger;
            _footswitchParser = new Fs3XParser();
            _checkStatusTimer = new Timer(OnCheckStatus, this, 0, 1000);
        }

        private void OnCheckStatus(object state)
        {
            if (this.IsConnected)
            {
                if (!this._serialPort.IsOpen)
                    this.Close();
            }
        }

        public void Connect()
        {
            if (!this.IsConnected)
            {
                try
                {
                    _serialPort = new SerialPort(this.CurrentPort.Id, CurrentSpeed, Parity.None, 8, StopBits.One);
                    _serialPort.DataReceived += SerialPortOnDataReceived;
                    _serialPort.Open();
                    this.IsConnected = true;
                }
                catch (Exception e)
                {
                    this.IsConnected = false;
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
                catch (Exception e)
                {
                }
                this.IsConnected = false;
            }
        }

        public void Close()
        {
            if (this.IsConnected)
            {
                _serialPort.DataReceived -= SerialPortOnDataReceived;
                _serialPort.Close();
                _serialPort.Dispose();
                this.IsConnected = false;
            }
        }

        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            foreach(var line in indata.Split('\n'))
                _messenger.Send(new ControllerStateMessage { States = _footswitchParser.ParseState(line, 0), Port = 0 });
        }

        public List<InputCOMPort> GetPorts()
        {
            var result = new List<InputCOMPort>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    result.Add(new InputCOMPort()
                    {
                        Name = queryObj["Name"].ToString(),
                        Id = queryObj["DeviceID"].ToString(),
                        MaxBaudRate = Int32.Parse(queryObj["MaxBaudRate"].ToString())
                    });
                }
                if(!result.Any())
                    result.Add(new InputCOMPort() { Name = "No COM ports available...", Id = "", MaxBaudRate = 0});

            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
            return result;
        }

        public List<int> GetRates()
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
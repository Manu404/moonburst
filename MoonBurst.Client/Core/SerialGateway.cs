using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Model;

namespace MoonBurst.ViewModel
{
    public enum FootswitchState
    {
        On, Off
    }
    public class Footswitch
    {
        public FootswitchState State { get; set; }
        public int  Index { get; set; }

        public Footswitch(int index)
        {
            State = FootswitchState.Off;
            Index = index;
        }
    }

    public interface IParser
    {
        List<Footswitch> ParseState(string message);
    }

    public class FS3XParser  : IParser
    {
        public List<Footswitch> ParseState(string message)
        {
            message = message.Trim();
            if(String.Equals("10", message))
                return new List<Footswitch>
                {
                    new Footswitch(0) { State = FootswitchState.On},
                    new Footswitch(1),
                    new Footswitch(2),
                };

            if (String.Equals("01", message))
                return new List<Footswitch>
                {
                    new Footswitch(0),
                    new Footswitch(1) { State = FootswitchState.On},
                    new Footswitch(2),
                };

            if (String.Equals("11", message))
                return new List<Footswitch>
                {
                    new Footswitch(0),
                    new Footswitch(1),
                    new Footswitch(2) { State = FootswitchState.On},
                };
            return new List<Footswitch>
            {
                new Footswitch(0),
                new Footswitch(1),
                new Footswitch(2),
            };
        }
    }

    public interface ISerialGateway
    {
        bool IsConnected { get; }
        int CurrentSpeed { get; set; }
        InputCOMPort CurrentPort { get; set; }
        List<InputCOMPort> GetPorts();
        List<int> GetRates();
        void Connect();
    }

    public class SerialGateway : ISerialGateway
    {
        private IParser _parser;
        private IMessenger _messenger;
        private SerialPort _serialPort;
        private bool _isConnected;

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
            _parser = new FS3XParser();
        }

        public void Connect()
        {
            if (!this.IsConnected)
            {
                try
                {
                    _serialPort = new SerialPort(this.CurrentPort.Name, CurrentSpeed, Parity.None, 8, StopBits.One);
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

        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            _parser.ParseState(indata);
        }

        public List<InputCOMPort> GetPorts()
        {
            var result = new List<InputCOMPort>();
            string[] ports = SerialPort.GetPortNames();
            if (ports.Any())
            {
                int i = 0;
                foreach (var port in ports)
                {
                    result.Add(new InputCOMPort() { Name = port, Id = i });
                    i++;
                }
            }
            else
            {
                result.Add(new InputCOMPort() { Name = "No COM ports available...", Id = -1 });
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
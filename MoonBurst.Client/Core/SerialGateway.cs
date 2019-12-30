using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Model;

namespace MoonBurst.ViewModel
{
    public enum FootswitchState
    {
        [Description("Pressing")]
        Pressing = 0,
        [Description("Pressed")]
        Pressed = 1,
        [Description("Releasing")]
        Releasing = 2,
        [Description("Released")]
        Released = 3,
        [Description("Unknown")]
        Unknown = 4
    }

    public interface IControllerInputState
    {
        int Index { get; }
    }

    public interface IFootswitchState  : IControllerInputState
    {
        FootswitchState State { get; }
    }

    public class MomentaryFootswitchState : IFootswitchState
    {
        public FootswitchState State { get; }
        public int  Index { get; }

        public MomentaryFootswitchState(FootswitchState state, int index)
        {
            State = state;
            Index = index;
        }
    }

    public interface IControllerInputParser
    {
        IControllerInputState ParseState(string state, int index);
    }

    public interface IFootswitchParser : IControllerInputParser
    {
        new IFootswitchState ParseState(string state, int index);
    }

    public interface IControllerParser
    {
        IDeviceDefinition Device { get; }
        List<MomentaryFootswitchState> ParseState(string state, int index);
        bool ValidateState(string state);
    }

    public class MomentaryFootswitchParser : IFootswitchParser
    {
        private int previous;

        public IFootswitchState ParseState(string state, int index)
        {
            int isPressed, wasPressed = previous;
            if(Int32.TryParse(state, out isPressed))
            {
                previous = isPressed;
                if (isPressed == 1)
                {
                    if(wasPressed == 1) return new MomentaryFootswitchState(FootswitchState.Pressed, index);
                    return new MomentaryFootswitchState(FootswitchState.Pressing, index);
                }
                else
                {
                    if (wasPressed == 0) return new MomentaryFootswitchState(FootswitchState.Released, index);
                    return new MomentaryFootswitchState(FootswitchState.Releasing, index);
                }
            }
            return new MomentaryFootswitchState(FootswitchState.Unknown, index);
        }

        IControllerInputState IControllerInputParser.ParseState(string state, int index)
        {
            return ParseState(state, index);
        }
    }

    public class Fs3XParser : IControllerParser
    {
        readonly MomentaryFootswitchParser[] _parser = new MomentaryFootswitchParser[3];

        public Fs3XParser()
        {
            for(int i = 0; i < 3; i++)
                _parser[i] = new MomentaryFootswitchParser();
        }

        public IDeviceDefinition Device => new Fs3xDeviceDefinition();

        public List<MomentaryFootswitchState> ParseState(string state, int index)
        {
            var result = new List<MomentaryFootswitchState>();
            if (state.Length != 4) return result;
            for(int i = 0; i < 3; i++)
                result.Add((MomentaryFootswitchState) _parser[i].ParseState(state[i].ToString(), i));
            return result;
        }

        public bool ValidateState(string state)
        {
            return true;
        }
    }

    public class ControllerStateMessage : MessageBase
    {
        public List<MomentaryFootswitchState> States { get; set; }
        public int Port { get; set; }
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
        private IControllerParser _footswitchParser;
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
            _footswitchParser = new Fs3XParser();
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
            foreach(var line in indata.Split('\n'))
                _messenger.Send(new ControllerStateMessage { States = _footswitchParser.ParseState(line, 0), Port = 0 });
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
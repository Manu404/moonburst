using System;
using System.Collections.Generic;
using MoonBurst.Api.Enums;
using MoonBurst.Api.Gateway;
using MoonBurst.Api.Gateway.Midi;
using Sanford.Multimedia.Midi;
using ChannelCommand = Sanford.Multimedia.Midi.ChannelCommand;
using MidiDevice = MoonBurst.Api.Gateway.Midi.MidiDevice;

namespace MoonBurst.Core.Gateway
{

    public class MidiGateway : IMidiGateway, IGateway
    {
        private OutputDevice _outDevice;
/*
        private TeVirtualMIDI _virtualMidi;
*/
        private readonly ChannelMessageBuilder _builder;
        private MidiConnectionState _state;
        private bool _isConnected;

        public MidiDevice SelectedOutput { get; set; }

        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                _isConnected = value;
                MidiConnectionState newState = value ? MidiConnectionState.Connected : MidiConnectionState.Disconnected;
                if(newState != _state)
                {
                    ConnectionStateChanged?.Invoke(this, new MidiConnectionStateChangedEventArgs(newState, _state) );
                    _state = newState;
                }
            }
        }

        public event EventHandler<MidiConnectionStateChangedEventArgs> ConnectionStateChanged;

        public MidiGateway()
        {
            _state = MidiConnectionState.Disconnected;
            _builder = new ChannelMessageBuilder();
        }

        public void Trigger(MidiTriggerData obj)
        {
            if (IsConnected && obj != null)
            {

                _builder.Command = (ChannelCommand)obj.Command;
                _builder.MidiChannel = obj.MidiChannel - 1;
                _builder.Data1 = obj.Data1 - 1;
                _builder.Data2 = obj.Data2 - 1;
                _builder.Build();

                _outDevice.Send(_builder.Result);
            }
        }

        public void Connect()
        {
            if (!IsConnected && SelectedOutput?.Id >= 0)
            {
                _outDevice = new OutputDevice(SelectedOutput.Id);
                IsConnected = true;
            }
            else
            {
                Close();
            }
        }

        public void Close()
        {
            _outDevice?.Close();
            IsConnected = false;
        }

        public void SendTest()
        {
            _builder.Command = ChannelCommand.NoteOn;
            _builder.MidiChannel = 1;
            _builder.Data1 = 64;
            _builder.Data2 = 127;
            _builder.Build();

            _outDevice.Send(_builder.Result);
        }

        public string GetStatusString()
        {
            if (this.IsConnected)
                return $"Connected to: {this.SelectedOutput.Name}";
            else
                return "Midi: Disconnected";
        }


        public List<MidiDevice> GetDevices()
        {
            var result = new List<MidiDevice>();
            if (OutputDeviceBase.DeviceCount > 0)
            {
                for (var i = 0; i < OutputDeviceBase.DeviceCount; i++)
                {
                    result.Add(new MidiDevice(i) { Name = OutputDeviceBase.GetDeviceCapabilities(i).name });
                }
            }
            else
            {
                //WriteLine("No devices found... :(");
                result.Add(new MidiDevice(-1) { Name = "No device output devices available..."});
            }

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using MoonBurst.Api.Enums;
using MoonBurst.Api.Gateways;
using Sanford.Multimedia.Midi;
using ChannelCommand = Sanford.Multimedia.Midi.ChannelCommand;

namespace MoonBurst.Core.Hardware
{

    public class MidiGateway : IMidiGateway, IGateway
    {
        private OutputDevice _outDevice;
/*
        private TeVirtualMIDI _virtualMidi;
*/
        private ChannelMessageBuilder _builder;
        private MidiConnectionState _state;
        private bool _isConnected;

        public OutputMidiDeviceData SelectedOutput { get; set; }

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


        public List<OutputMidiDeviceData> GetDevices()
        {
            var result = new List<OutputMidiDeviceData>();
            if (OutputDeviceBase.DeviceCount > 0)
            {
                for (var i = 0; i < OutputDeviceBase.DeviceCount; i++)
                {
                    result.Add(new OutputMidiDeviceData(i) { Name = OutputDeviceBase.GetDeviceCapabilities(i).name });
                }
            }
            else
            {
                //WriteLine("No devices found... :(");
                result.Add(new OutputMidiDeviceData(-1) { Name = "No device output devices available..."});
            }

            return result;
        }
    }
}
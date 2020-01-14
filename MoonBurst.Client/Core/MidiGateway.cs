using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using MoonBurst.Core;
using MoonBurst.Model;
using MoonBurst.Model.Messages;
using MoonBurst.ViewModel;
using Sanford.Multimedia.Midi;

namespace MoonBurst.Core
{
    public interface IMidiGateway
    {
        OutputMidiDevice SelectedOutput { get; set; }
        bool IsConnected { get; }
        void Trigger(TriggeredActionMessage obj);
        void Connect();
        void SendTest();
        void Close();
        List<OutputMidiDevice> GetDevices();
    }

    public class MidiGateway : IMidiGateway
    {
        private OutputDevice _outDevice;
        private TeVirtualMIDI _virtualMidi;
        private IMessenger _messenger;
        private ChannelMessageBuilder _builder;
        private MidiConnectionStatus _state;
        private bool _isConnected;

        public OutputMidiDevice SelectedOutput { get; set; }

        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                _isConnected = value;
                MidiConnectionStatus newState = value ? MidiConnectionStatus.Connected : MidiConnectionStatus.Disconnected;
                if(newState != _state)
                {
                    _messenger.Send(new MidiConnectionStateChangedMessage() { NewState = newState, PreviousState = _state});
                    _state = newState;
                }
            }
        }

        public MidiGateway(IMessenger messenger)
        {
            _messenger = messenger;
            _state = MidiConnectionStatus.Disconnected;
            _builder = new ChannelMessageBuilder();

            _messenger.Register<TriggeredActionMessage>(this, Trigger);
        }

        public void Trigger(TriggeredActionMessage obj)
        {
            if (this.IsConnected && obj.Data != null)
            {

                _builder.Command = (Sanford.Multimedia.Midi.ChannelCommand)obj.Data.Command;
                _builder.MidiChannel = obj.Data.MidiChannel - 1;
                _builder.Data1 = obj.Data.Data1 - 1;
                _builder.Data2 = obj.Data.Data2 - 1;
                _builder.Build();

                this._outDevice.Send(_builder.Result);
            }
        }

        public void Connect()
        {
            if (!this.IsConnected && this.SelectedOutput?.Id >= 0)
            {
                _outDevice = new OutputDevice(this.SelectedOutput.Id);
                this.IsConnected = true;
            }
            else
            {
                Close();
            }
        }

        public void Close()
        {

            _outDevice?.Close();
            this.IsConnected = false;
        }

        public void SendTest()
        {
            _builder.Command = Sanford.Multimedia.Midi.ChannelCommand.NoteOn;
            _builder.MidiChannel = 1;
            _builder.Data1 = 64;
            _builder.Data2 = 127;
            _builder.Build();

            this._outDevice.Send(_builder.Result);
        }


        public List<OutputMidiDevice> GetDevices()
        {
            var result = new List<OutputMidiDevice>();
            if (OutputDevice.DeviceCount > 0)
            {
                for (int i = 0; i < OutputDevice.DeviceCount; i++)
                {
                    result.Add(new OutputMidiDevice() { Name = OutputDevice.GetDeviceCapabilities(i).name, Id = i });
                }
            }
            else
            {
                //WriteLine("No devices found... :(");
                result.Add(new OutputMidiDevice() { Name = "No device output devices available...", Id = -1 });
            }

            return result;
        }
    }
}
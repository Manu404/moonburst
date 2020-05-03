using System;
using System.Collections.Generic;

namespace MoonBurst.Api.Gateways
{
    public interface IMidiGateway
    {
        MidiDevice SelectedOutput { get; set; }
        bool IsConnected { get; }
        event EventHandler<MidiConnectionStateChangedEventArgs> ConnectionStateChanged;
        void Connect();
        void Close();
        List<MidiDevice> GetDevices();
        void Trigger(MidiTriggerData obj);
        void SendTest();
        string GetStatusString();
    }
}
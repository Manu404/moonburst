using System.Collections.Generic;
using MoonBurst.Model;
using MoonBurst.Model.Messages;

namespace MoonBurst.Core
{
    public interface IMidiGateway
    {
        OutputMidiDeviceData SelectedOutput { get; set; }
        bool IsConnected { get; }
        void Trigger(TriggeredActionMessage obj);
        void Connect();
        void SendTest();
        void Close();
        List<OutputMidiDeviceData> GetDevices();
    }
}
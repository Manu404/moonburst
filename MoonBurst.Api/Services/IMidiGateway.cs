using System.Collections.Generic;
using MoonBurst.Api.Enums;

namespace MoonBurst.Api.Services
{
    public class MidiTriggerData
    {
        public int MidiChannel { get; set; }
        public ChannelCommand Command { get; set; }
        public int Data1 { get; set; }
        public int Data2 { get; set; }
        public int Delay { get; set; }
    }

    public class OutputMidiDeviceData
    {
        public OutputMidiDeviceData()
        {
            
        }
        public OutputMidiDeviceData(int id)
        {
            Id = id;
        }

        public string Name { get; set; }
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is OutputMidiDeviceData)) return false;
            return Id == ((OutputMidiDeviceData)obj).Id;
        }

        protected bool Equals(OutputMidiDeviceData other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }

    public interface IMidiGateway
    {
        OutputMidiDeviceData SelectedOutput { get; set; }
        bool IsConnected { get; }
        void Trigger(MidiTriggerData obj);
        void Connect();
        void SendTest();
        void Close();
        List<OutputMidiDeviceData> GetDevices();
    }
}
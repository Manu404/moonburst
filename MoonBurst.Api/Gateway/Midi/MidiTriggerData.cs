using MoonBurst.Api.Enums;

namespace MoonBurst.Api.Gateway.Midi
{
    public class MidiTriggerData
    {
        public int MidiChannel { get; set; }
        public ChannelCommand Command { get; set; }
        public int Data1 { get; set; }
        public int Data2 { get; set; }
        public int Delay { get; set; }
    }
}
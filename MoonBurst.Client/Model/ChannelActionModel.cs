using MoonBurst.Api.Enums;

namespace MoonBurst.Model
{
    public class ChannelActionModel
    {
        public int MidiChannel { get; set; }
        public int Data1 { get; set; }
        public int Data2 { get; set; }
        public ChannelCommand Command { get; set; }
        public FootTrigger Trigger { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsLocked { get; set; }
    }
}
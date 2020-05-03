using System.Collections.Generic;

namespace MoonBurst.Model
{
    public class LayoutChannelModel
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsLocked { get; set; }
        public string BindedInput { get; set; }

        public List<ChannelActionModel> Actions { get; set; }

        public LayoutChannelModel()
        {
            Actions = new List<ChannelActionModel>();
        }
    }
}
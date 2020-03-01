using System.Collections.Generic;

namespace MoonBurst.ViewModel
{
    public class LayoutData
    {
        public List<FunctoidChannelData> Channels { get; set; }
        public LayoutData()
        {
            Channels = new List<FunctoidChannelData>();
        }
    }
}
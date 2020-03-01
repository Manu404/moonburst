using System.Collections.Generic;

namespace MoonBurst.ViewModel
{
    public class LayoutData
    {
        public List<FunctoidChannelViewModel.FunctoidChannelData> Channels { get; set; }
        public LayoutData()
        {
            Channels = new List<FunctoidChannelViewModel.FunctoidChannelData>();
        }
    }
}
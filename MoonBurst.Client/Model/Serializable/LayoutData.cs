using System.Collections.Generic;
using MoonBurst.ViewModel;

namespace MoonBurst.Model.Serializable
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
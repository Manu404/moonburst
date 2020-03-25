using System.Collections.Generic;

namespace MoonBurst.Model
{
    public class LayoutModel
    {
        public List<FunctoidChannelModel> Channels { get; set; }
        public LayoutModel()
        {
            Channels = new List<FunctoidChannelModel>();
        }
    }
}
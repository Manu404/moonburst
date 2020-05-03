using System.Collections.Generic;
using System.Linq;

namespace MoonBurst.Model
{
    public class LayoutModel
    {
        public List<LayoutChannelModel> Channels { get; set; }
        public LayoutModel()
        {
            Channels = new List<LayoutChannelModel>();
        }
    }
}
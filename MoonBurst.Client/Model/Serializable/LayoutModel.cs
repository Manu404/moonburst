using System.Collections.Generic;
using MoonBurst.ViewModel;

namespace MoonBurst.Model.Serializable
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
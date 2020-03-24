using System.Collections.Generic;
using MoonBurst.ViewModel;

namespace MoonBurst.Model.Serializable
{
    public class FunctoidChannelData
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsExpanded { get; set; }
        public string BindedInput { get; set; }

        public List<FunctoidActionData> Actions { get; set; }

        public FunctoidChannelData()
        {
            Actions = new List<FunctoidActionData>();
        }
    }
}
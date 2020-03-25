using System.Collections.Generic;

namespace MoonBurst.Model
{
    public class FunctoidChannelModel
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsExpanded { get; set; }
        public string BindedInput { get; set; }

        public List<FunctoidActionModel> Actions { get; set; }

        public FunctoidChannelModel()
        {
            Actions = new List<FunctoidActionModel>();
        }
    }
}
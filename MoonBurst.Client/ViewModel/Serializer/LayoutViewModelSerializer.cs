using System.Linq;
using MoonBurst.Core;

namespace MoonBurst.ViewModel
{
    public partial class LayoutViewModel
    {
        public partial class LayoutViewModelSerializer : SerializerBase<LayoutViewModel, LayoutData>
        {
            public override string Default { get => "default_layout.xml"; }

            public override LayoutData ExtractData(LayoutViewModel source)
            {
                return new LayoutData()
                {
                    Channels = source.FunctoidChannels.ToList().ConvertAll(f => f.GetData())
                };
            }

            public override void ApplyData(LayoutData config, LayoutViewModel target)
            {
                target.UpdateChannels(config.Channels);
            }
        }


    }
}
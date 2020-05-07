using Jacobi.Vst.Framework;
using MoonBurst.Api.Client;

namespace MoonBurst.Vst
{
    public interface IPluginMidiSourceFactory : IFactory<IVstPluginMidiSource, IVstHost>
    {
    }

    public class PluginMidiSourceFactory : IPluginMidiSourceFactory
    {
        public IVstPluginMidiSource Build(IVstHost host)
        {
            return new PluginMidiSource(host);
        }
    }

    class PluginMidiSource : IVstPluginMidiSource
    {
        private readonly IVstHost _host;

        public PluginMidiSource(IVstHost host)
        {
            _host = host;
        }

        public int ChannelCount
        {
            get
            {
                IVstMidiProcessor midiProcessor = null;
                
                if(_host != null)
                {
                    midiProcessor = _host.GetInstance<IVstMidiProcessor>();
                }

                if (midiProcessor != null)
                {
                    return midiProcessor.ChannelCount;
                }

                return 0;
            }
        }
    }
}
using Jacobi.Vst.Framework;
using MoonBurst.Api.Client;

namespace MoonBurst.Vst
{
    public interface IPluginMidiSourceFactory : IFactory<IVstPluginMidiSource, IVstHost>
    {
    }

    public class PluginMidiSourceFactory : IPluginMidiSourceFactory
    {
        private static IVstPluginMidiSource _instance;

        public IVstPluginMidiSource Build(IVstHost host)
        {
            if(_instance == null) _instance = new PluginMidiSource(host);
            return _instance;
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
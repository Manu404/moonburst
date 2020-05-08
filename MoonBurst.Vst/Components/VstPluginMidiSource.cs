using Jacobi.Vst.Framework;
using MoonBurst.Api.Client;

namespace MoonBurst.Vst
{
    public interface IPluginMidiSourceFactory : IFactory<IVstPluginMidiSource, IVstHost>
    {
    }

    public class VstPluginMidiSourceFactory : IPluginMidiSourceFactory
    {
        private IVstPluginMidiSource _instance;
        public IVstPluginMidiSource Build(IVstHost host)
        {
            if(_instance == null) _instance = new VstPluginMidiSource(host);
            return _instance;
        }
    }

    class VstPluginMidiSource : IVstPluginMidiSource
    {
        private readonly IVstHost _host;

        public VstPluginMidiSource(IVstHost host)
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
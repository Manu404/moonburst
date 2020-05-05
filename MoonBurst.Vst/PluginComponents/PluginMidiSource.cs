using Jacobi.Vst.Framework;

namespace MoonBurst.Vst
{
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
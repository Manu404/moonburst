using Jacobi.Vst.Framework;

namespace MoonBurst.Vst
{
    class PluginMidiSource : IVstPluginMidiSource
    {
        private Plugin _plugin;

        public PluginMidiSource(Plugin plugin)
        {
            _plugin = plugin;
        }

        public int ChannelCount
        {
            get
            {
                IVstMidiProcessor midiProcessor = null;
                
                if(_plugin.Host != null)
                {
                    midiProcessor = _plugin.Host.GetInstance<IVstMidiProcessor>();
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
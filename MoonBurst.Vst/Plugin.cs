using MoonBurst.Api.Client;

namespace MoonBurst.Vst
{
    using Jacobi.Vst.Core;
    using Jacobi.Vst.Framework;
    using Jacobi.Vst.Framework.Plugin;
    using System.Reflection;

    /// <summary>
    /// The Plugin root class that implements the interface manager and the plugin midi source.
    /// </summary>
    class Plugin : VstPluginWithInterfaceManagerBase, IVstPluginMidiSource
    {
        private readonly ILauncher _launcher;

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public Plugin(ILauncher launcher)
            : base("MoonBurst", 
                new VstProductInfo("MoonBurst " + Assembly.GetExecutingAssembly().GetName().Version, "Emmanuel Istace", 1001),
                VstPluginCategory.Generator, 
                VstPluginCapabilities.NoSoundInStop, 
                0, 
                0x30313234)
        {
            this._launcher = launcher;
            launcher.Initialize();
        }

        protected override IVstPluginAudioProcessor CreateAudioProcessor(IVstPluginAudioProcessor instance)
        {
            if (instance == null) return new AudioProcessor(this);

            return instance;
        }

        protected override IVstPluginEditor CreateEditor(IVstPluginEditor instance)
        {
            if (instance == null) return new PluginEditor(this, _launcher.Host);

            return instance;
        }

        protected override IVstMidiProcessor CreateMidiProcessor(IVstMidiProcessor instance)
        {
            if (instance == null) return new MidiProcessor(this);

            return instance;
        }

        protected override IVstPluginPersistence CreatePersistence(IVstPluginPersistence instance)
        {
            if (instance == null) return new PluginPersistence(this);

            return instance;
        }

        protected override IVstPluginMidiSource CreateMidiSource(IVstPluginMidiSource instance)
        {
            return this;
        }

        public int ChannelCount
        {
            get
            {
                IVstMidiProcessor midiProcessor = null;
                
                if(Host != null)
                {
                    midiProcessor = Host.GetInstance<IVstMidiProcessor>();
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

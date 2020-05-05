using System;
using System.Windows.Forms;
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
    class Plugin : VstPluginWithInterfaceManagerBase 
    {
        private readonly IApp _app;

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public Plugin(IApp app)
            : base("MoonBurst", 
                new VstProductInfo("MoonBurst " + Assembly.GetExecutingAssembly().GetName().Version, "Emmanuel Istace", 1001),
                VstPluginCategory.Generator, 
                VstPluginCapabilities.NoSoundInStop, 
                0, 
                0x30313234)
        {
            this._app = app;
            this._app.Initialize();
        }

        protected override IVstPluginAudioProcessor CreateAudioProcessor(IVstPluginAudioProcessor instance)
        {
            if (instance == null) instance = new AudioProcessor(this);
            return instance;
        }

        protected override IVstPluginEditor CreateEditor(IVstPluginEditor instance)
        {
            if (instance == null) instance = new PluginEditor(this, _app.Host);
            return instance;
        }

        protected override IVstMidiProcessor CreateMidiProcessor(IVstMidiProcessor instance)
        {
            if (instance == null) instance = new MidiProcessor(this.GetInstance<IVstPluginMidiSource>());
            return instance;
        }

        protected override IVstPluginPersistence CreatePersistence(IVstPluginPersistence instance)
        {
            if (instance == null) instance = new PluginPersistence(this);
            return instance;
        }

        protected override IVstPluginMidiSource CreateMidiSource(IVstPluginMidiSource instance)
        {
            if (instance == null) instance = new PluginMidiSource(this);
            return instance;
        }
    }
}

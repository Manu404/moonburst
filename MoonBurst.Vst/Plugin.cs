using System;
using System.Windows.Forms;
using MoonBurst.Api.Client;

namespace MoonBurst.Vst
{
    using Jacobi.Vst.Core;
    using Jacobi.Vst.Framework;
    using Jacobi.Vst.Framework.Plugin;
    using System.Reflection;

    class Plugin : VstPluginWithInterfaceManagerBase 
    {
        private readonly IApp _app;

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
            if (this.Host == null) return instance;
            if (instance == null) instance = new AudioProcessor(this.GetInstance<MidiProcessor>(), this.Host.GetInstance<IVstMidiProcessor>());
            return instance;
        }

        protected override IVstPluginEditor CreateEditor(IVstPluginEditor instance)
        {
            if (instance == null) instance = new PluginEditor(_app.Host);
            return instance;
        }

        protected override IVstMidiProcessor CreateMidiProcessor(IVstMidiProcessor instance)
        {
            if (instance == null) instance = new MidiProcessor(this.GetInstance<IVstPluginMidiSource>());
            return instance;
        }

        protected override IVstPluginPersistence CreatePersistence(IVstPluginPersistence instance)
        {
            if (instance == null) instance = new PluginPersistence();
            return instance;
        }

        protected override IVstPluginMidiSource CreateMidiSource(IVstPluginMidiSource instance)
        {
            if (this.Host == null) return instance;
            if (instance == null) instance = new PluginMidiSource(this.Host);
            return instance;
        }
    }
}

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
        private readonly IVstPluginPersistence _persistence;
        private readonly IPluginMidiSourceFactory _midiSourceFactory;
        private readonly IMidiProcessorFactory _midiProcessorFactory;
        private readonly IPluginEditorFactory _pluginEditorFactory;
        private readonly IAudioProcessorFactory _audioProcessorFactory;

        public Plugin(IApp app,
            IVstPluginPersistence persistence,
            IPluginMidiSourceFactory midiSourceFactory, 
            IAudioProcessorFactory audioProcessorFactory, 
            IPluginEditorFactory pluginEditorFactory, 
            IMidiProcessorFactory midiProcessorFactory)
            : base("MoonBurst", 
                new VstProductInfo("MoonBurst " + Assembly.GetExecutingAssembly().GetName().Version, "Emmanuel Istace", 1001),
                VstPluginCategory.Generator, 
                VstPluginCapabilities.NoSoundInStop, 
                0, 
                0x30313234)
        {
            this._app = app;
            this._persistence = persistence;
            this._midiSourceFactory = midiSourceFactory;
            this._audioProcessorFactory = audioProcessorFactory;
            this._pluginEditorFactory = pluginEditorFactory;
            this._midiProcessorFactory = midiProcessorFactory;
            this._app.Initialize();
        }

        protected override IVstPluginAudioProcessor CreateAudioProcessor(IVstPluginAudioProcessor instance)
        {
            if (this.Host == null) return instance;
            if (instance == null) instance = _audioProcessorFactory.Build(new Tuple<MidiProcessor, IVstMidiProcessor>(GetInstance<MidiProcessor>(), Host.GetInstance<IVstMidiProcessor>()));
            return instance;
        }

        protected override IVstPluginEditor CreateEditor(IVstPluginEditor instance)
        {
            if (instance == null) instance = _pluginEditorFactory.Build(_app.Host);
            return instance;
        }

        protected override IVstMidiProcessor CreateMidiProcessor(IVstMidiProcessor instance)
        {
            if (instance == null) instance = _midiProcessorFactory.Build(GetInstance<IVstPluginMidiSource>());
            return instance;
        }

        protected override IVstPluginPersistence CreatePersistence(IVstPluginPersistence instance)
        {
            if (instance == null) instance = _persistence;
            return instance;
        }

        protected override IVstPluginMidiSource CreateMidiSource(IVstPluginMidiSource instance)
        {
            if (this.Host == null) return instance;
            if (instance == null) instance = _midiSourceFactory.Build(Host);
            return instance;
        }
    }
}

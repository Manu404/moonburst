﻿using System;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;
using MoonBurst.Api.Client;

namespace MoonBurst.Vst
{
    public class PluginInterfaceManager : PluginInterfaceManagerBase
    {
        private readonly IApp _app;
        private readonly IVstHost _host;
        private readonly IVstPluginPersistence _persistence;
        private readonly IPluginMidiSourceFactory _midiSourceFactory;
        private readonly IMidiProcessorFactory _midiProcessorFactory;
        private readonly IPluginEditorFactory _pluginEditorFactory;
        private readonly IAudioProcessorFactory _audioProcessorFactory;

        public PluginInterfaceManager(IApp app, 
            IVstHost host,
            IVstPluginPersistence persistence,
            IPluginMidiSourceFactory midiSourceFactory,
            IAudioProcessorFactory audioProcessorFactory,
            IPluginEditorFactory pluginEditorFactory,
            IMidiProcessorFactory midiProcessorFactory)
        {
            this._app = app;
            this._host = host;
            this._persistence = persistence;
            this._midiSourceFactory = midiSourceFactory;
            this._audioProcessorFactory = audioProcessorFactory;
            this._pluginEditorFactory = pluginEditorFactory;
            this._midiProcessorFactory = midiProcessorFactory;
        }


        protected override IVstPluginAudioProcessor CreateAudioProcessor(IVstPluginAudioProcessor instance)
        {
            if (this._host == null) return instance;
            if (instance == null) instance = _audioProcessorFactory.Build(new Tuple<MidiProcessor, IVstMidiProcessor>(GetInstance<MidiProcessor>(), _host.GetInstance<IVstMidiProcessor>()));
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
            if (this._host == null) return instance;
            if (instance == null) instance = _midiSourceFactory.Build(_host);
            return instance;
        }
    }
}
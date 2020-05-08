using System;
using System.Windows.Forms;
using MoonBurst.Api.Client;
using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;
using System.Reflection;

namespace MoonBurst.Vst
{
    public interface IPluginFactory : IFactory<IVstPlugin>
    {

    }

    public class PluginFactory : IPluginFactory
    {
        private readonly IAppFactory _appFactory;
        private readonly IPluginInterfaceManagerFactory _pluginInterfaceFactory;
        private static IVstPlugin _instance;

        public PluginFactory(IAppFactory appFactory,
            IPluginInterfaceManagerFactory pluginInterfaceFactory)
        {
            this._appFactory = appFactory;
            _pluginInterfaceFactory = pluginInterfaceFactory;
        }

        public IVstPlugin Build()
        {
            if(_instance == null) _instance = new Plugin(_appFactory, _pluginInterfaceFactory);
            return _instance;
        }
    }

    public interface IPluginInterfaceManagerFactory : IFactory<PluginInterfaceManager, Tuple<IApp, IVstHost>>
    {

    }
    public class PluginInterfaceManagerFactory : IPluginInterfaceManagerFactory
    {
        private readonly IVstPluginPersistence _persistence;
        private readonly IPluginMidiSourceFactory _midiSourceFactory;
        private readonly IMidiProcessorFactory _midiProcessorFactory;
        private readonly IPluginEditorFactory _pluginEditorFactory;
        private readonly IAudioProcessorFactory _audioProcessorFactory;

        public PluginInterfaceManagerFactory(
            IVstPluginPersistence persistence,
            IPluginMidiSourceFactory midiSourceFactory,
            IAudioProcessorFactory audioProcessorFactory,
            IPluginEditorFactory pluginEditorFactory,
            IMidiProcessorFactory midiProcessorFactory)
        {
            this._persistence = persistence;
            this._midiSourceFactory = midiSourceFactory;
            this._audioProcessorFactory = audioProcessorFactory;
            this._pluginEditorFactory = pluginEditorFactory;
            this._midiProcessorFactory = midiProcessorFactory;
        }

        public PluginInterfaceManager Build(Tuple<IApp, IVstHost> data)
        {
            return new PluginInterfaceManager(data.Item1, data.Item2, _persistence, _midiSourceFactory, _audioProcessorFactory, _pluginEditorFactory, _midiProcessorFactory);
        }
    }

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

    class Plugin : VstPluginBase
    {
        private readonly IApp _app;
        private readonly PluginInterfaceManager _interfaceManager;

        public Plugin(IAppFactory appFactory,
            IPluginInterfaceManagerFactory interfaceManagerFactory)
            : base("MoonBurst", 
                new VstProductInfo("MoonBurst " + Assembly.GetExecutingAssembly().GetName().Version, "Emmanuel Istace", 1001),
                VstPluginCategory.Generator, 
                VstPluginCapabilities.NoSoundInStop, 
                0, 
                0x30313234)
        {
            this._app = appFactory.Build();
            this._app.Initialize();
            _interfaceManager = interfaceManagerFactory.Build(new Tuple<IApp, IVstHost>(_app, this.Host));
        }

        public override bool Supports<T>()
        {
            return _interfaceManager.Supports<T>();
        }
        public override T GetInstance<T>()
        {
            return _interfaceManager.GetInstance<T>();
        }
    }
}

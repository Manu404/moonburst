using System;
using Jacobi.Vst.Framework;
using MoonBurst.Api.Client;

namespace MoonBurst.Vst
{
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
}
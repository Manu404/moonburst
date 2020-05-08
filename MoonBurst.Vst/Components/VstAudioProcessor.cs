using System;
using MoonBurst.Api.Client;
using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

namespace MoonBurst.Vst
{
    public interface IAudioProcessorFactory : IFactory<VstPluginAudioProcessorBase, Tuple<VstMidiProcessor, IVstMidiProcessor>>
    {
    }

    public class VstAudioProcessorFactory : IAudioProcessorFactory
    {
        private VstAudioProcessor _instance;
        public VstPluginAudioProcessorBase Build(Tuple<VstMidiProcessor, IVstMidiProcessor> data)
        {
            if(_instance == null)_instance = new VstAudioProcessor(data.Item1, data.Item2);
            return _instance;
        }
    }

    /// <summary>
    /// A dummy audio processor only used for the timing of midi processing.
    /// </summary>
    class VstAudioProcessor : VstPluginAudioProcessorBase
    {
        private VstPlugin _vstPlugin;
        private VstMidiProcessor _vstMidiProcessor;
        private IVstMidiProcessor _hostProcessor;

        public VstAudioProcessor(VstMidiProcessor processor, IVstMidiProcessor host)
            : base(0, 0, 0)
        {
            _hostProcessor = host;
            _vstMidiProcessor = processor;
        }

        public override void Process(VstAudioBuffer[] inChannels, VstAudioBuffer[] outChannels)
        {
            if (_vstMidiProcessor != null && _hostProcessor != null &&
                _vstMidiProcessor.Events.Count > 0)
            {
                _hostProcessor.Process(_vstMidiProcessor.Events);
                _vstMidiProcessor.Events.Clear();
            }

            // perform audio-through
            base.Process(inChannels, outChannels);
        }
    }
}

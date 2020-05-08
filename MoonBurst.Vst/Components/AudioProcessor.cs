using System;
using MoonBurst.Api.Client;
using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

namespace MoonBurst.Vst
{
    public interface IAudioProcessorFactory : IFactory<VstPluginAudioProcessorBase, Tuple<MidiProcessor, IVstMidiProcessor>>
    {
    }

    public class AudioProcessorFactory : IAudioProcessorFactory
    {
        private static AudioProcessor _instance;
        public VstPluginAudioProcessorBase Build(Tuple<MidiProcessor, IVstMidiProcessor> data)
        {
            if(_instance == null)_instance = new AudioProcessor(data.Item1, data.Item2);
            return _instance;
        }
    }

    /// <summary>
    /// A dummy audio processor only used for the timing of midi processing.
    /// </summary>
    class AudioProcessor : VstPluginAudioProcessorBase
    {
        private Plugin _plugin;
        private MidiProcessor _midiProcessor;
        private IVstMidiProcessor _hostProcessor;

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="plugin">Must not be null.</param>
        public AudioProcessor(MidiProcessor processor, IVstMidiProcessor host)
            : base(0, 0, 0)
        {
            _hostProcessor = host;
            _midiProcessor = processor;
        }

        /// <inheritdoc />
        /// <remarks>This method is used to push midi events to the host.</remarks>
        public override void Process(VstAudioBuffer[] inChannels, VstAudioBuffer[] outChannels)
        {
            if (_midiProcessor != null && _hostProcessor != null &&
                _midiProcessor.Events.Count > 0)
            {
                _hostProcessor.Process(_midiProcessor.Events);
                _midiProcessor.Events.Clear();
            }

            // perform audio-through
            base.Process(inChannels, outChannels);
        }
    }
}

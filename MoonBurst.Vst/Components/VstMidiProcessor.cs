﻿using System.Collections;
using Jacobi.Vst.Core;
using MoonBurst.Api.Client;
using Jacobi.Vst.Framework;
using System.Collections.Generic;

namespace MoonBurst.Vst
{
    public interface IMidiProcessorFactory : IFactory<IVstMidiProcessor, IVstPluginMidiSource>
    {
    }

    public class VstMidiProcessorFactory : IMidiProcessorFactory
    {
        private IVstMidiProcessor _instance;
        public IVstMidiProcessor Build(IVstPluginMidiSource source)
        {
            if(_instance == null) _instance = new VstMidiProcessor(source);
            return _instance;
        }
    }

    public class VstMidiProcessor : IVstMidiProcessor
    {
        private IVstPluginMidiSource _plugin;

        public VstMidiProcessor(IVstPluginMidiSource plugin)
        {
            _plugin = plugin;
            Events = new VstEventCollection();
            NoteOnEvents = new Queue<byte>();
        }

        /// <summary>
        /// Gets the midi events that should be processed in the current cycle.
        /// </summary>
        public VstEventCollection Events { get; }

        public bool MidiThru { get; set; }
        public Queue<byte> NoteOnEvents { get; }
        
        public int ChannelCount
        {
            get { return _plugin.ChannelCount; }
        }

        public void Process(VstEventCollection events)
        {
            foreach (VstEvent evnt in events)
            {
                if (evnt.EventType != VstEventTypes.MidiEvent) continue;
                VstMidiEvent midiEvent = (VstMidiEvent)evnt;

                if (((midiEvent.Data[0] & 0xF0) == 0x80 || (midiEvent.Data[0] & 0xF0) == 0x90))
                {
                    // add original event
                    Events.Add(evnt);
                }
            }
        }
    }
}
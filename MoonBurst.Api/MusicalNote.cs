using System.Collections.Generic;

namespace MoonBurst.Api
{
    public interface INoteNameFormatter
    {
        string GetPitchName(int midiValue);
        string GetName(int midiValue);
    }

    public interface INoteHelper
    {
        IList<MusicalNote> AvailableNotes { get; }
    }

    public class MusicalNote
    {
        private INoteNameFormatter[] availableFormatters;
        public int MidiValue { get; }
        public string Name
        {
            get { return availableFormatters[1].GetName(MidiValue); }
        }
        public string DisplayNameDetailed { get => $"{Name} ({MidiValue})"; }

        public MusicalNote(INoteNameFormatter[] availableFormatters, int i)
        {
            this.availableFormatters = availableFormatters;
            MidiValue = i;
        }

    }
}
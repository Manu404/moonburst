using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonBurst.Core.Helper
{
    public interface IMusicalNoteHelper
    {
        List<MusicalNote> AvailableNotes { get; }
    }

    public class MusicalNote
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class MusicalNoteHelper : IMusicalNoteHelper
    {
        public List<MusicalNote> AvailableNotes { get; }

        public MusicalNoteHelper()
        {
            AvailableNotes = new List<MusicalNote>();
            for(int i = 0; i < 128; i++)
            {
                AvailableNotes.Add(new MusicalNote() { Name = FromMidiValueToNoteName(i), Value = i });
            }
        }

        private string FromMidiValueToNoteName(int value)
        {
            if (value < 0 || value > 127) throw new Exception($"'{value}' is not a valid midi note value");

            int note = value % 12;
            int octave = (value > 12 ? (value - note) / 12 : 0);
            octave = octave - 1; // 0 = C-1, not C0         
            return $"{GetNoteValueName(note)}{octave}";
        }

        private string GetNoteValueName(int value)
        {
            if (value < 0 || value > 12) throw new Exception($"'{value}' is not a valid note value within an octave");
            switch (value)
            {
                case 0: return "C";
                case 1: return "C#";
                case 2: return "D";
                case 3: return "D#";
                case 4: return "E";
                case 5: return "F";
                case 6: return "F#";
                case 7: return "G";
                case 8: return "G#";
                case 9: return "A";
                case 10: return "A#";
                case 11: return "B";
                default: return "unknown";
            }
        }
    }

}

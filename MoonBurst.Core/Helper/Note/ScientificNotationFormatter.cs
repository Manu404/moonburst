using System;

namespace MoonBurst.Core.Helper
{
    public abstract class ScientificNotationFormatter
    {
        public abstract string GetPitchName(int midiValue);

        public string GetName(int midiValue)
        {
            if (midiValue < 0 || midiValue > 127) throw new Exception($"'{midiValue}' is not a valid midi note value");

            int note = midiValue % 12;
            int octave = (midiValue > 12 ? (midiValue - note) / 12 : 0);
            octave -= 1; // 0 = C-1, not C0         
            return $"{GetPitchName(note)}{octave}";
        }
    }
}
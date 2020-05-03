using System;
using MoonBurst.Api;

namespace MoonBurst.Core.Helper
{
    public class EnglishNotationFormatter : ScientificNotationFormatter, INoteNameFormatter
    {
        public override string GetPitchName(int value)
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
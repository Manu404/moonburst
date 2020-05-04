using System;
using MoonBurst.Api.Helper;

namespace MoonBurst.Core.Helper
{
    public class LatinNotationFormatter : ScientificNotationFormatter, INoteNameFormatter
    {
        public override string GetPitchName(int value)
        {
            if (value < 0 || value > 12) throw new Exception($"'{value}' is not a valid note value within an octave");
            switch (value)
            {
                case 0: return "Do ";
                case 1: return "Do# ";
                case 2: return "Re ";
                case 3: return "Re# ";
                case 4: return "Mi ";
                case 5: return "Fa ";
                case 6: return "Fa# ";
                case 7: return "Sol ";
                case 8: return "Sol# ";
                case 9: return "La ";
                case 10: return "La# ";
                case 11: return "Si ";
                default: return "unknown";
            }
        }
    }
}
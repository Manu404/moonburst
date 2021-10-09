using System;

namespace MoonBurst.Api.Helper
{
    public class FormatedNote
    {
        private int currentFormatter = 0;
        private readonly INoteNameFormatter[] availableFormatters;
        public int MidiValue { get; }
        public string Name => availableFormatters[currentFormatter].GetName(MidiValue);
        public string DisplayNameDetailed => $"{Name} ({MidiValue})";

        public FormatedNote(INoteNameFormatter[] availableFormatters, int i)
        {
            this.availableFormatters = availableFormatters;
            MidiValue = i;
        }

        public void NextFormatter()
        {
            currentFormatter += 1;
            if (currentFormatter >= availableFormatters.Length)
                currentFormatter = 0;
        }
    }
}
namespace MoonBurst.Api.Helper
{
    public class FormatedNote
    {
        private readonly INoteNameFormatter[] availableFormatters;
        public int MidiValue { get; }
        public string Name => availableFormatters[1].GetName(MidiValue);
        public string DisplayNameDetailed => $"{Name} ({MidiValue})";

        public FormatedNote(INoteNameFormatter[] availableFormatters, int i)
        {
            this.availableFormatters = availableFormatters;
            MidiValue = i;
        }

    }
}
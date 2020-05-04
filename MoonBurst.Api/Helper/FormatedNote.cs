namespace MoonBurst.Api
{
    public class FormatedNote
    {
        private INoteNameFormatter[] availableFormatters;
        public int MidiValue { get; }
        public string Name
        {
            get { return availableFormatters[1].GetName(MidiValue); }
        }
        public string DisplayNameDetailed { get => $"{Name} ({MidiValue})"; }

        public FormatedNote(INoteNameFormatter[] availableFormatters, int i)
        {
            this.availableFormatters = availableFormatters;
            MidiValue = i;
        }

    }
}
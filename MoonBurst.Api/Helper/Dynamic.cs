namespace MoonBurst.Api.Helper
{
    public class Dynamic
    {
        public Dynamic(string name, int midiValue, string display)
        {
            Name = name;
            MidiValue = midiValue;
            Display = display;
        }
        
        public string Name { get; }
        public int MidiValue { get; }
        public string Display { get; }
        
        public string ValueDisplay => $"\f({MidiValue})";
    }
}
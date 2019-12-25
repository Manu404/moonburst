namespace MoonBurst.Model
{
    public class OutputMidiDevice
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if(!(obj is  OutputMidiDevice)) return false;
            return Id == ((OutputMidiDevice)obj).Id;
        }
    }
}
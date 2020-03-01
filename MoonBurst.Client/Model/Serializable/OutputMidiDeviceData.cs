namespace MoonBurst.Model
{
    public class OutputMidiDeviceData
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if(!(obj is  OutputMidiDeviceData)) return false;
            return Id == ((OutputMidiDeviceData)obj).Id;
        }
    }
}
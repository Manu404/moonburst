namespace MoonBurst.Api.Gateways
{
    public class MidiDevice
    {
        public MidiDevice()
        {
            
        }
        public MidiDevice(int id)
        {
            Id = id;
        }

        public string Name { get; set; }
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is MidiDevice)) return false;
            return Id == ((MidiDevice)obj).Id;
        }

        protected bool Equals(MidiDevice other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
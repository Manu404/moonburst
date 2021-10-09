namespace MoonBurst.Api.Gateway.Serial
{
    public class ComPort
    {
        public ComPort()
        {
            
        }
        
        public ComPort(string id)
        {
            Id = id;
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public int MaxBaudRate { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ComPort)) return false;
            if (Id == null) return false; 
            return Id.Equals(((ComPort)obj).Id);
        }

        protected bool Equals(ComPort other)
        {
            return (Id != null && other?.Id != null) && Id == other?.Id;
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}
namespace MoonBurst.Api.Gateway.Serial
{
    public class ComPortSpeed
    {
        public ComPortSpeed()
        {

        }

        public ComPortSpeed(int speed)
        {
            BaudRate = speed;
        }
        
        public int BaudRate { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ComPortSpeed)) return false;
            return BaudRate.Equals(((ComPortSpeed)obj).BaudRate);
        }

        protected bool Equals(ComPortSpeed other)
        {
            return other?.BaudRate != null && BaudRate == other?.BaudRate;
        }

        public override int GetHashCode()
        {
            return (BaudRate.GetHashCode());
        }
    }
}
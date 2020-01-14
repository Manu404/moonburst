namespace MoonBurst.Model
{
    public class InputCOMPort
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int MaxBaudRate { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is InputCOMPort)) return false;
            return Id.Equals(((InputCOMPort)obj).Id);
        }
    }
}
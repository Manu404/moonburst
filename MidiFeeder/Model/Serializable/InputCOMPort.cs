namespace MoonBurst.Model
{
    public class InputCOMPort
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is InputCOMPort)) return false;
            return Name.Equals(((InputCOMPort)obj).Name);
        }
    }
}
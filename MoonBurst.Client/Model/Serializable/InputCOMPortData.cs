namespace MoonBurst.Model
{
    public class InputCOMPortData
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int MaxBaudRate { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is InputCOMPortData)) return false;
            return Id.Equals(((InputCOMPortData)obj).Id);
        }
    }
}
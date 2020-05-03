namespace MoonBurst.Api
{
    public class Dynamic
    {
        public Dynamic(string name, int value, string display)
        {
            Name = name;
            Value = value;
            Display = display;
        }
        
        public string Name { get; }
        public int Value { get; }
        public string Display { get; }
        
        public string ValueDisplay => $"\f({Value})";
    }
}
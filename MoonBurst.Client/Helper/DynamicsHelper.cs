using System.Collections.Generic;

namespace MoonBurst.Core.Helper
{
    public class Dynamic
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string Display { get; set; }
        public string ValueDisplay => $" - ({Value})";
    }

    public interface IDynamicsHelper
    {
        List<Dynamic> AvailableDynamics { get; }
    }

    public class DynamicsHelper : IDynamicsHelper
    {
        public List<Dynamic> AvailableDynamics { get; }

        public DynamicsHelper()
        {
            int[] midi = new int[] { 10, 23, 36, 49, 62, 75, 88, 101, 114, 127 };
            string[] name = new string[] { "pppp", "ppp", "pp", "p", "mp", "mf", "f", "ff", "fff", "ffff" };
            string[] display = new string[] { "OO", "OP", "O", "P", "mP", "mF", "F", "D", "DF", "DD" };

            AvailableDynamics = new List<Dynamic>();

            for(int i = 0; i < midi.Length; i++)
            {
                AvailableDynamics.Add(new Dynamic()
                {
                    Display = display[i],
                    Name = name[i],
                    Value = midi[i],
                });
            }
        }
    }

}

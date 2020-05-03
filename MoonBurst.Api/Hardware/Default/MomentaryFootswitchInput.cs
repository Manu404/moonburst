using MoonBurst.Api.Hardware;

namespace MoonBurst.Core.Arduino
{
    public class MomentaryFootswitchInput : IBooleanInput
    {
        public int Position { get; }
        public string Name { get; }

        public MomentaryFootswitchInput(int position, string name)
        {
            Position = position;
            Name = name;
        }
    }
}
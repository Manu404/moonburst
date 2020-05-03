using MoonBurst.Api.Hardware.Description;

namespace MoonBurst.Api.Hardware.Default
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
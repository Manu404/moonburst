using System.Collections.Generic;
using MoonBurst.Api.Hardware;

namespace MoonBurst.Core.Hardware.Arduino
{
    public class Fs3XDeviceDefinition : IDeviceDefinition
    {
        public IEnumerable<IDeviceInput> GetInputs()
        {
            return new List<IDeviceInput>
            {
                new MomentaryFootswitchInput(0, "Mode"),
                new MomentaryFootswitchInput(1, "Down"),
                new MomentaryFootswitchInput(2, "Up")
            };
        }

        public string Name => "FS3X Footswitch";
        public string Description => "Digitech FS3X Footswitch device";
    }
}
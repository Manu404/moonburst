using MoonBurst.Api.Hardware;
using System.Collections.Generic;

namespace MoonBurst.Core
{
    public class Fs3xDeviceDefinition : IDeviceDefinition
    {
        public IList<IDeviceInput> GetInputs()
        {
            return new List<IDeviceInput>()
            {
                new MomentaryFootswitchInput(0, "Mode"),
                new MomentaryFootswitchInput(1, "Down"),
                new MomentaryFootswitchInput(2, "Up"),
            };
        }

        public string Name => "FS3X Footswitch";
        public string Description => "Digitech FS3X Footswitch device";
    }
}
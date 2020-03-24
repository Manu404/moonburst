using System.Collections.Generic;
using MoonBurst.Api.Hardware;

namespace MoonBurst.Core.Hardware.Arduino
{
    public class GenericOneMomentarySwitchMonoJackDefinition : IDeviceDefinition
    {
        public IEnumerable<IDeviceInput> GetInputs()
        {
            return new List<IDeviceInput>
            {
                new MomentaryFootswitchInput(0,  "Switch")
            };
        }

        public string Name => "Generic Momentary Switch";
        public string Description => "Generic Momentary Switch Mono Connector";
    }
}
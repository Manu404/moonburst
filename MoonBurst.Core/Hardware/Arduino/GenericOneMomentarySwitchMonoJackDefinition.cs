using MoonBurst.Api.Hardware;
using System.Collections.Generic;

namespace MoonBurst.Core
{
    public class GenericOneMomentarySwitchMonoJackDefinition : IDeviceDefinition
    {
        public IList<IDeviceInput> GetInputs()
        {
            return new List<IDeviceInput>()
            {
                new MomentaryFootswitchInput(0,  "Switch")
            };
        }

        public string Name => "Generic Momentary Switch";
        public string Description => "Generic Momentary Switch Mono Connector";
    }
}
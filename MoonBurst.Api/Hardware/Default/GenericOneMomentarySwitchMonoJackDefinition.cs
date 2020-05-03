using System.Collections.Generic;
using MoonBurst.Api.Hardware;
using MoonBurst.Api.Parser;

namespace MoonBurst.Core.Arduino
{
    public class GenericOneMomentarySwitchMonoJackDefinitionParser : IDeviceParser
    {
        public IDeviceDefinition Device { get; }

        public IDeviceInputState[] ParseState(int state, int index)
        {
            return new IDeviceInputState[0];
        }

        public bool ValidateState(string state)
        {
            return true;
        }
    }

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

        public IDeviceParser BuildParser()
        {
            return new GenericOneMomentarySwitchMonoJackDefinitionParser();
        }
    }
}
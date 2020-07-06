using System.Collections.Generic;
using System.Diagnostics;
using MoonBurst.Api.Hardware.Description;
using MoonBurst.Api.Hardware.Parser;

namespace MoonBurst.Api.Hardware.Default
{
    public class GenericTwoMomentarySwitchStereoJack_Parser : IDeviceParser
    {
        private const int SwitchCount = 2;
        private readonly IFootswitchParser[] _parser = new IFootswitchParser[SwitchCount];

        public GenericTwoMomentarySwitchStereoJack_Parser()
        {
            for (var i = 0; i < SwitchCount; i++)
                _parser[i] = new MomentaryFootswitchParser();
        }

        public IDeviceInputState[] ParseState(int state, int index)
        {
            Debug.WriteLine(state);
            return new IDeviceInputState[]
            {
                _parser[0].ParseState((state == 1) ? 1 : 0, 0),
                _parser[1].ParseState((state == 2) ? 1 : 0, 1)
            };
        }

        public bool ValidateState(string state)
        {
            return true;
        }
    }

    public class GenericTwoMomentarySwitchStereoJack_Definition : IDeviceDefinition
    {
        public IEnumerable<IDeviceInput> GetInputs()
        {
            return new List<IDeviceInput>
             {
                 new MomentaryFootswitchInput(0, "A"),
                 new MomentaryFootswitchInput(1, "B")
             };
        }

        public string Name => "Generic Two Momentary Switch";
        public string Description => "Generic two momentary switch on stereo connector";

        public IDeviceParser BuildParser()
        {
            return new GenericTwoMomentarySwitchStereoJack_Parser();
        }
    }
}
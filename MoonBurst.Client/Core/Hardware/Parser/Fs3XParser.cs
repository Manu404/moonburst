using System.Collections.Generic;
using MoonBurst.Model.Parser;

namespace MoonBurst.Core.Parser
{
    public class Fs3XParser : IControllerParser
    {
        private const int SwitchCount = 3;
        readonly MomentaryFootswitchParser[] _parser = new MomentaryFootswitchParser[SwitchCount];

        public Fs3XParser()
        {
            for(int i = 0; i < SwitchCount; i++)
                _parser[i] = new MomentaryFootswitchParser();
        }

        public IDeviceDefinition Device => new Fs3xDeviceDefinition();

        public MomentaryFootswitchState[] ParseState(int state, int index)
        {
            return new MomentaryFootswitchState[SwitchCount]
            {
                ((MomentaryFootswitchState) _parser[0].ParseState((state == 1) ? 1 : 0, 0)),
                ((MomentaryFootswitchState) _parser[1].ParseState((state == 2) ? 1 : 0, 1)),
                ((MomentaryFootswitchState) _parser[2].ParseState((state == 0) ? 1 : 0, 2))
            };
        }

        public bool ValidateState(string state)
        {
            return true;
        }
    }
}
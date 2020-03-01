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

        public MomentaryFootswitchState[] ParseState(string state, int index)
        {
            var result = new MomentaryFootswitchState[SwitchCount];
            if (state.Length != SwitchCount) return new MomentaryFootswitchState[0];
            for(int i = 0; i < SwitchCount; i++)
                result[i] = ((MomentaryFootswitchState) _parser[i].ParseState(state[i].ToString(), i));
            return result;
        }

        public bool ValidateState(string state)
        {
            return true;
        }
    }
}
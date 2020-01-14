using System.Collections.Generic;
using MoonBurst.Model.Parser;

namespace MoonBurst.Core.Parser
{
    public class Fs3XParser : IControllerParser
    {
        readonly MomentaryFootswitchParser[] _parser = new MomentaryFootswitchParser[3];

        public Fs3XParser()
        {
            for(int i = 0; i < 3; i++)
                _parser[i] = new MomentaryFootswitchParser();
        }

        public IDeviceDefinition Device => new Fs3xDeviceDefinition();

        public List<MomentaryFootswitchState> ParseState(string state, int index)
        {
            var result = new List<MomentaryFootswitchState>();
            if (state.Length != 4) return result;
            for(int i = 0; i < 3; i++)
                result.Add((MomentaryFootswitchState) _parser[i].ParseState(state[i].ToString(), i));
            return result;
        }

        public bool ValidateState(string state)
        {
            return true;
        }
    }
}
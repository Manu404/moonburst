using MoonBurst.Api.Hardware;
using MoonBurst.Api.Parser;
using MoonBurst.Core.Hardware.Parser;

namespace MooBurst.Parser.Fs3X
{
    public class Fs3XParser : IDeviceParser
    {
        private const int SwitchCount = 3;
        private readonly MomentaryFootswitchParser[] _parser = new MomentaryFootswitchParser[SwitchCount];

        public Fs3XParser()
        {
            for(var i = 0; i < SwitchCount; i++)
                _parser[i] = new MomentaryFootswitchParser();
        }
        
        public IDeviceInputState[] ParseState(int state, int index)
        {
            return new[]
            {
                (FootswitchState) _parser[0].ParseState((state == 1) ? 1 : 0, 0),
                (FootswitchState) _parser[1].ParseState((state == 2) ? 1 : 0, 1),
                (FootswitchState) _parser[2].ParseState((state == 0) ? 1 : 0, 2)
            };
        }

        public bool ValidateState(string state)
        {
            return true;
        }
    }
}
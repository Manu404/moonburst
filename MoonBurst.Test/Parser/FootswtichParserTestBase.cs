using System.Collections.Generic;
using MoonBurst.Api.Hardware.Parser;

namespace MoonBurst.Test
{
    public abstract class FootswtichParserTestBase
    {
        public List<FootswitchStates> GetSequence()
        {
            return new List<FootswitchStates>()
            {
                FootswitchStates.Released,
                FootswitchStates.Pressing,
                FootswitchStates.Pressed,
                FootswitchStates.Releasing,
                FootswitchStates.Released
            };
        }
    }
}
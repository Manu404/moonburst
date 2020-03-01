using System;
using MoonBurst.Model.Parser;

namespace MoonBurst.Core.Parser
{
    public class MomentaryFootswitchParser : IFootswitchParser
    {
        private int previous;

        public IFootswitchState ParseState(string state, int index)
        {
            int isPressed, wasPressed = previous;
            if(Int32.TryParse(state, out isPressed))
            {
                previous = isPressed;
                if (isPressed == 1)
                {
                    if(wasPressed == 1) return new MomentaryFootswitchState(FootswitchState.Pressed, index);
                    return new MomentaryFootswitchState(FootswitchState.Pressing, index);
                }
                else
                {
                    if (wasPressed == 0) return new MomentaryFootswitchState(FootswitchState.Released, index);
                    return new MomentaryFootswitchState(FootswitchState.Releasing, index);
                }
            }
            return new MomentaryFootswitchState(FootswitchState.Unknown, index);
        }

        IControllerInputState IControllerInputParser.ParseState(string state, int index)
        {
            return ParseState(state, index);
        }
    }
}
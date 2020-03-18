using System;
using MoonBurst.Model.Parser;

namespace MoonBurst.Core.Parser
{
    public class MomentaryFootswitchParser : IFootswitchParser
    {
        private int previous;

        public IFootswitchState ParseState(int state, int index)
        {
            int isPressed = state, wasPressed = previous;
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

        IControllerInputState IControllerInputParser.ParseState(int state, int index)
        {
            return ParseState(state, index);
        }
    }
}
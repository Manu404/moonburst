using MoonBurst.Api.Parser;

namespace MoonBurst.Core.Hardware.Parser
{
    public class MomentaryFootswitchParser : IFootswitchParser
    {
        private int _previous;

        public IFootswitchState ParseState(int state, int index)
        {
            int isPressed = state, wasPressed = _previous;
            _previous = isPressed;
            if (isPressed == 1)
            {
                return wasPressed == 1 
                    ? new MomentaryFootswitchState(FootswitchState.Pressed, index) 
                    : new MomentaryFootswitchState(FootswitchState.Pressing, index);
            }
            return wasPressed == 0 
                ? new MomentaryFootswitchState(FootswitchState.Released, index) 
                : new MomentaryFootswitchState(FootswitchState.Releasing, index);
        }

        IControllerInputState IControllerInputParser.ParseState(int state, int index)
        {
            return ParseState(state, index);
        }
    }
}
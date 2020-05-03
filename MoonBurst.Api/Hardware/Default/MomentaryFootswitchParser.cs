using MoonBurst.Api.Hardware.Parser;

namespace MoonBurst.Api.Hardware.Default
{
    public class MomentaryFootswitchParser : IMomentaryFootswitchParser
    {
        private int _previous;

        public IFootswitchState ParseState(int state, int index)
        {
            int isPressed = state, wasPressed = _previous;
            _previous = isPressed;
            if (isPressed == 1)
            {
                return wasPressed == 1 
                    ? new FootswitchState(FootswitchStates.Pressed, index) 
                    : new FootswitchState(FootswitchStates.Pressing, index);
            }
            return wasPressed == 0 
                ? new FootswitchState(FootswitchStates.Released, index) 
                : new FootswitchState(FootswitchStates.Releasing, index);
        }

        IDeviceInputState IDeviceInputParser.ParseState(int state, int index)
        {
            return ParseState(state, index);
        }
    }
}
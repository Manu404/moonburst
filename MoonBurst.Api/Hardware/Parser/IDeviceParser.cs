using MoonBurst.Api.Hardware;

namespace MoonBurst.Api.Parser
{
    public interface IDeviceParser
    {
        IDeviceInputState[] ParseState(int state, int index);
        bool ValidateState(string state);
    }
}
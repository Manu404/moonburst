using MoonBurst.Api.Hardware;

namespace MoonBurst.Api.Parser
{
    public interface IDeviceParser
    {
        IDeviceDefinition Device { get; }
        MomentaryFootswitchState[] ParseState(int state, int index);
        bool ValidateState(string state);
    }
}
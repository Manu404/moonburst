using System.Collections.Generic;
using MoonBurst.Api.Hardware;
using MoonBurst.Core;

namespace MoonBurst.Api.Parser
{
    public interface IControllerParser
    {
        IDeviceDefinition Device { get; }
        MomentaryFootswitchState[] ParseState(int state, int index);
        bool ValidateState(string state);
    }
}
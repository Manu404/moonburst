using System.Collections.Generic;
using MoonBurst.Core;

namespace MoonBurst.Model.Parser
{
    public interface IControllerParser
    {
        IDeviceDefinition Device { get; }
        MomentaryFootswitchState[] ParseState(string state, int index);
        bool ValidateState(string state);
    }
}
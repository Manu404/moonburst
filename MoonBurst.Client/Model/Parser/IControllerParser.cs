using System.Collections.Generic;
using MoonBurst.Core;

namespace MoonBurst.Model.Parser
{
    public interface IControllerParser
    {
        IDeviceDefinition Device { get; }
        MomentaryFootswitchState[] ParseState(int state, int index);
        bool ValidateState(string state);
    }
}
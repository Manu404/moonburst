using System.Collections.Generic;
using MoonBurst.Api.Hardware.Description;
using MoonBurst.Api.Hardware.Parser;

namespace MoonBurst.Api.Hardware
{
    public interface IDeviceDefinition
    {
        IEnumerable<IDeviceInput> GetInputs();
        string Name { get; }
        string Description { get; }
        IDeviceParser BuildParser();
    }
}
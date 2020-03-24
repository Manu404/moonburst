using System.Collections.Generic;

namespace MoonBurst.Api.Hardware
{
    public interface IDeviceDefinition
    {
        IEnumerable<IDeviceInput> GetInputs();
        string Name { get; }
        string Description { get; }
    }
}
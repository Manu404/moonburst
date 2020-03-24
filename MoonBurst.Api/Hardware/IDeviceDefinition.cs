using System.Collections.Generic;

namespace MoonBurst.Api.Hardware
{
    public interface IDeviceDefinition
    {
        IList<IDeviceInput> GetInputs();
        string Name { get; }
        string Description { get; }
    }
}
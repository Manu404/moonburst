using System.Collections.Generic;

namespace MoonBurst.Core
{
    public interface IDeviceDefinition
    {
        IList<IDeviceInput> GetInputs();
        string Name { get; }
        string Description { get; }
    }
}
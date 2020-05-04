using System.Collections.Generic;

namespace MoonBurst.Api.Helper
{
    public interface IDynamicsHelper : IHelper
    {
        List<Dynamic> AvailableDynamics { get; }
    }
}
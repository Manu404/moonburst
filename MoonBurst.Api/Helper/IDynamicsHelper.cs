using System.Collections.Generic;
using MoonBurst.Api;

namespace MoonBurst.Core.Helper
{
    public interface IDynamicsHelper : IHelper
    {
        List<Dynamic> AvailableDynamics { get; }
    }
}
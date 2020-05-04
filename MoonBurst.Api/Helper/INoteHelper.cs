using System.Collections.Generic;

namespace MoonBurst.Api
{
    public interface INoteHelper : IHelper
    {
        IList<FormatedNote> AvailableNotes { get; }
    }
}
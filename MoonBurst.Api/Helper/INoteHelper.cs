using System.Collections.Generic;

namespace MoonBurst.Api.Helper
{
    public interface INoteHelper : IHelper
    {
        IList<FormatedNote> AvailableNotes { get; }
    }
}
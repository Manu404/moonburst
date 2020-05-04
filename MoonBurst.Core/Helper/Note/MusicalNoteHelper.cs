using System.Collections.Generic;
using MoonBurst.Api.Helper;

namespace MoonBurst.Core.Helper
{
    public class NoteHelper : INoteHelper
    {
        public IList<FormatedNote> AvailableNotes { get; }

        public NoteHelper(INoteNameFormatter[] formatters)
        {
            AvailableNotes = new List<FormatedNote>();
            for(var i = 127; i >= 0; i--)
            {
                AvailableNotes.Add(new FormatedNote(formatters, i));
            }
        }
    }

}

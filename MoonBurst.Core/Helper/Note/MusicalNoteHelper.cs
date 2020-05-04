using System.Collections.Generic;
using System.Linq;
using MoonBurst.Api;

namespace MoonBurst.Core.Helper
{
    public class NoteHelper : INoteHelper
    {
        public IList<FormatedNote> AvailableNotes { get; }

        public NoteHelper(INoteNameFormatter[] formatters)
        {
            AvailableNotes = new List<FormatedNote>();
            for(var i = 0; i < 128; i++)
            {
                AvailableNotes.Add(new FormatedNote(formatters, i));
            }
            AvailableNotes.Reverse();
        }
    }

}

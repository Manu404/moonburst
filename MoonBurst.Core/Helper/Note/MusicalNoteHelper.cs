using System.Collections.Generic;
using System.Linq;
using MoonBurst.Api;

namespace MoonBurst.Core.Helper
{
    public class NoteHelper : INoteHelper
    {
        public IList<MusicalNote> AvailableNotes { get; }

        public NoteHelper(INoteNameFormatter[] formatters)
        {
            AvailableNotes = new List<MusicalNote>();
            for(var i = 0; i < 128; i++)
            {
                AvailableNotes.Add(new MusicalNote(formatters, i));
            }
            AvailableNotes.Reverse();
        }
    }

}

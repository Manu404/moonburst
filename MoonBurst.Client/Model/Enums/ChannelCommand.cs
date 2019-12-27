using System;

namespace MoonBurst.Model
{
    [Serializable]
    public enum ChannelCommand
    {
        None = 0,
        /// <summary>Represents the note-off command type.</summary>
        NoteOff = 128, // 0x00000080
        /// <summary>Represents the note-on command type.</summary>
        NoteOn = 144, // 0x00000090
        /// <summary>
        /// Represents the poly pressure (aftertouch) command type.
        /// </summary>
        PolyPressure = 160, // 0x000000A0
        /// <summary>Represents the controller command type.</summary>
        Controller = 176, // 0x000000B0
        /// <summary>Represents the program change command type.</summary>
        ProgramChange = 192, // 0x000000C0
        /// <summary>
        /// Represents the channel pressure (aftertouch) command
        /// type.
        /// </summary>
        ChannelPressure = 208, // 0x000000D0
        /// <summary>Represents the pitch wheel command type.</summary>
        PitchWheel = 224, // 0x000000E0
    }
}
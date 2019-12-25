using System;

namespace MoonBurst.Model
{
    [Serializable]
    public enum FootTrigger
    {
        Press = 0,
        Sustain = 1,
        Release = 2,
        On = 3, 
        Off = 4,
    }
}
using System;

namespace MoonBurst.Api.Enums
{
    [Serializable]
    public enum FootTrigger
    {
        Press = 0,
        Sustain = 1,
        Release = 2,
        Released = 3,
        On = 4, 
        Off = 5,
    }
}
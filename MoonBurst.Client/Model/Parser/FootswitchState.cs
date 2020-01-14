using System.ComponentModel;

namespace MoonBurst.Model.Parser
{
    public enum FootswitchState
    {
        [Description("Pressing")]
        Pressing = 0,
        [Description("Pressed")]
        Pressed = 1,
        [Description("Releasing")]
        Releasing = 2,
        [Description("Released")]
        Released = 3,
        [Description("Unknown")]
        Unknown = 4
    }
}
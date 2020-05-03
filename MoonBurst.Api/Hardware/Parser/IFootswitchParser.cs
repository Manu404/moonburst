namespace MoonBurst.Api.Hardware.Parser
{
    public interface IFootswitchParser : IDeviceInputParser
    {
        new IFootswitchState ParseState(int state, int index);
    }

    public interface IMomentaryFootswitchParser : IFootswitchParser
    {

    }
}
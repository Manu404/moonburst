namespace MoonBurst.Api.Parser
{
    public interface IFootswitchParser : IDeviceInputParser
    {
        new IFootswitchState ParseState(int state, int index);
    }

    public interface IMomentaryFootswitchParser : IFootswitchParser
    {

    }
}
namespace MoonBurst.Api.Parser
{
    public interface IDeviceInputParser
    {
        IDeviceInputState ParseState(int state, int index);
    }
}
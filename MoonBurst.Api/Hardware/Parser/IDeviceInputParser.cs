namespace MoonBurst.Api.Hardware.Parser
{
    public interface IDeviceInputParser
    {
        IDeviceInputState ParseState(int state, int index);
    }
}
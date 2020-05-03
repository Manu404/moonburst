namespace MoonBurst.Api.Hardware.Parser
{
    public interface IDeviceParser
    {
        IDeviceInputState[] ParseState(int state, int index);
        bool ValidateState(string state);
    }
}
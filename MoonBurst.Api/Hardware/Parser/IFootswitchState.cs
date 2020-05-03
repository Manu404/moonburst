namespace MoonBurst.Api.Hardware.Parser
{
    public interface IFootswitchState  : IDeviceInputState
    {
        FootswitchStates States { get; }
    }
}
namespace MoonBurst.Api.Parser
{
    public interface IFootswitchState  : IDeviceInputState
    {
        FootswitchStates States { get; }
    }
}
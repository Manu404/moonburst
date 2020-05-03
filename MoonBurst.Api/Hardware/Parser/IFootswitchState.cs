namespace MoonBurst.Api.Parser
{
    public interface IFootswitchState  : IDeviceInputState
    {
        FootswitchState State { get; }
    }
}
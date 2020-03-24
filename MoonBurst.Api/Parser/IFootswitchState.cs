namespace MoonBurst.Api.Parser
{
    public interface IFootswitchState  : IControllerInputState
    {
        FootswitchState State { get; }
    }
}
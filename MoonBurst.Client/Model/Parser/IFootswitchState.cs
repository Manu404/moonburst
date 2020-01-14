namespace MoonBurst.Model.Parser
{
    public interface IFootswitchState  : IControllerInputState
    {
        FootswitchState State { get; }
    }
}
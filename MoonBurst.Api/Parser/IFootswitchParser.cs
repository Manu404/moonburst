namespace MoonBurst.Api.Parser
{
    public interface IFootswitchParser : IControllerInputParser
    {
        new IFootswitchState ParseState(int state, int index);
    }
}
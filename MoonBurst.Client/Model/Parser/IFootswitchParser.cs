namespace MoonBurst.Model.Parser
{
    public interface IFootswitchParser : IControllerInputParser
    {
        new IFootswitchState ParseState(int state, int index);
    }
}
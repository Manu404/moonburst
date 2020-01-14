namespace MoonBurst.Model.Parser
{
    public interface IFootswitchParser : IControllerInputParser
    {
        new IFootswitchState ParseState(string state, int index);
    }
}
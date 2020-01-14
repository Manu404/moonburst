namespace MoonBurst.Model.Parser
{
    public interface IControllerInputParser
    {
        IControllerInputState ParseState(string state, int index);
    }
}
namespace MoonBurst.Api.Parser
{
    public interface IControllerInputParser
    {
        IControllerInputState ParseState(int state, int index);
    }
}
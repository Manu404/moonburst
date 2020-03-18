namespace MoonBurst.Model.Parser
{
    public interface IControllerInputParser
    {
        IControllerInputState ParseState(int state, int index);
    }
}
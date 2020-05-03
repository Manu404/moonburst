namespace MoonBurst.Api.Hardware.Parser
{
    public class FootswitchState : IFootswitchState
    {
        public FootswitchStates States { get; }
        public int  Index { get; }

        public FootswitchState(FootswitchStates states, int index)
        {
            States = states;
            Index = index;
        }
    }
}
namespace MoonBurst.Api.Parser
{
    public class MomentaryFootswitchState : IFootswitchState
    {
        public FootswitchState State { get; }
        public int  Index { get; }

        public MomentaryFootswitchState(FootswitchState state, int index)
        {
            State = state;
            Index = index;
        }
    }
}
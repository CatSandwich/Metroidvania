using UnityEngine;

namespace Entity.State_Machine
{
    public class MetroidTransition<TStateEnum>
    {
        public delegate bool TransitionDel(StateMachineContext c);
        public readonly TransitionDel ShouldTransition;

        public readonly TStateEnum Current;
        public readonly TStateEnum Destination;

        public MetroidTransition(TStateEnum current, TStateEnum destination, TransitionDel shouldTransition)
        {
            Current = current;
            Destination = destination;
            ShouldTransition = shouldTransition;
        }
    }
}
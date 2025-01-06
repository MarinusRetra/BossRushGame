namespace BossRush.FiniteStateMachine.Behaviors
{
    /// <summary>
    /// Abstract State class acting as the parent for all the behaviors.
    /// </summary>
    public class State : IState
    {
        protected StateMachine Machine;

        public State(StateMachine machine)
        {
            Machine = machine;
        }

        public virtual void Enter()
        { }

        public virtual void Exit()
        { }
        
        public virtual void Update()
        { }

        public virtual void FixedUpdate()
        { }
    }
}

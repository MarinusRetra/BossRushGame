namespace BossRush.FiniteStateMachine.Behaviors
{
    /// <summary>
    /// Abstract State class acting as the parent for all the behaviors.
    /// </summary>
    public abstract class State : IState
    {
        protected StateMachine Machine;

        protected State(StateMachine machine)
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

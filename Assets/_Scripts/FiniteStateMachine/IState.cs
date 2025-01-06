namespace BossRush.FiniteStateMachine
{
    public interface IState
    {
        /// <summary>
        /// Called upon entering the current state.
        /// </summary>
        void Enter();

        /// <summary>
        /// Called upon exiting the current state.
        /// </summary>
        void Exit();

        /// <summary>
        /// Called every frame, updates the entity in this current state.
        /// </summary>
        void Update();

        /// <summary>
        /// Called every fixed interval, updates the entity in this current state.
        /// </summary>
        void FixedUpdate();
    }
}

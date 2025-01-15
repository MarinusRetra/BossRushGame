using UnityEngine;

namespace BossRush.FiniteStateMachine.Behaviors.Bosses
{
    /// <summary>
    /// A shockwave ability that deals damage around the entity.
    /// </summary>
    [System.Serializable]
    public class Shockwave : State
    {
        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public float MaxBounds { get; private set; }

        public Shockwave(StateMachine machine) : base(machine)
        { }

        public override void Enter()
        {
            // Perform enter animation
        }

        public override void Exit()
        {
            
        }

        private void OnEnterComplete()
        {
            // Interpolate the initial circle outwards

            // Deal damage to any entity in this circle

            // Disable the circle once it's reached the max bounds
        }
    }
}

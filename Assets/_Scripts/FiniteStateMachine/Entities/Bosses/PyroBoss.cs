using BossRush.FiniteStateMachine.Behaviors.Bosses;
using UnityEngine;

namespace BossRush.FiniteStateMachine.Entities.Bosses
{
    /// <summary>
    /// 
    /// </summary>
    public class PyroBoss : BossEntity
    {
        [field: Header("Behaviors")] 
        [SerializeField, Tooltip("The shockwave behavior, viewable and able to modify certain properties in inspector during runtime.")]
        private Shockwave _shockwave;

        [field: SerializeField] public AnimationClip ShockwaveClip { get; private set; }

        [field: Space, SerializeField, Tooltip("The toroid prefab that is being used by the shockwave.")]
        public GameObject ToroidObject { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();

            // Initialize behaviors
            _shockwave = new Shockwave(Machine, this);

            // Set the new state
            Machine.SetState(_shockwave);
        }
    }
}

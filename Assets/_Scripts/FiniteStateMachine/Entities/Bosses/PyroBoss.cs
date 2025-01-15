using BossRush.FiniteStateMachine.Behaviors.Bosses;
using Unity.Netcode.Components;
using UnityEngine;

namespace BossRush.FiniteStateMachine.Entities.Bosses
{
    /// <summary>
    /// 
    /// </summary>
    public class PyroBoss : BossEntity
    {
        [field: Header("Behaviors")] 
        [field: SerializeField, Tooltip("A reference to the current target's transform.")]
        private NetworkTransform _targetTransform;

        [SerializeField, Tooltip("The shockwave behavior, viewable and able to modify certain properties in inspector during runtime.")]
        private Shockwave _shockwave;

        [field: SerializeField] public AnimationClip ShockwaveClip { get; private set; }

        [field: Space, SerializeField, Tooltip("The toroid prefab that is being used by the shockwave.")]
        public GameObject ToroidObject { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();

            // Initialize behaviors
            _shockwave = new Shockwave(Machine, this)
            {
                ExpansionSpeed = 1.2f
            };

            // Set the new state
            Machine.SetState(_shockwave);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Machine.SetState(_shockwave);
            }
        }

        /// <summary>
        /// Set a new target for the boss to follow.
        /// </summary>
        /// <param name="newTarget"></param>
        public void SetNewTarget(NetworkTransform newTarget)
        {
            _targetTransform = newTarget;
        }
    }
}

using UnityEngine;

using BossRush.FiniteStateMachine.Entities.Bosses;

namespace BossRush.FiniteStateMachine.Behaviors.Bosses
{
    /// <summary>
    /// A shockwave ability that deals damage around the entity.
    /// </summary>
    [System.Serializable]
    public class Shockwave : State
    {
        // Runtime configurations
        [field: Header("Bounds config.")]
        [field: SerializeField, Tooltip("The current radius the shockwave toroid is at now.")]
        public float Radius { get; set; } = 1f;

        [field: SerializeField, Tooltip("The amount of meters the shockwave can go until breaking apart.")]
        public float MaxRadius { get; set; } = 30f;

        [field: SerializeField, Tooltip("The speed the toroid expands at."), Range(1f, 30f)]
        public float ExpansionSpeed { get; set; }

        // State attributes
        private GameObject _toroid;

        private static readonly int IS_SHOCKWAVE_COMPLETE = Animator.StringToHash("IsShockwaveComplete");
        private AnimationClip _clip;

        // To activate or deactivate the toroid
        private bool _active;

        public Shockwave(StateMachine machine) : base(machine)
        { }

        public Shockwave(StateMachine machine, PyroBoss pb) : base(machine)
        {
            // initialize the toroid and animation clip
            _toroid = pb.ToroidObject;
            _clip = pb.ShockwaveClip;
        }

        public override void Enter()
        {
            // Perform enter animation
            var animator = Machine.GetBlackboardReference().Animator;
            if (animator != null)
            {
                animator.Animator.SetBool(IS_SHOCKWAVE_COMPLETE, false);
                animator.Animator.Play(nameof(_clip));
            }
        }

        public override void Update()
        {
            // Perform enter animation
            var animator = Machine.GetBlackboardReference().Animator;
            if (animator != null && animator.Animator.GetBool(IS_SHOCKWAVE_COMPLETE))
            {
                animator.Animator.SetBool(IS_SHOCKWAVE_COMPLETE, false);
                OnAnimationEnded();
            }

            // increase scale up to the max, but only if the game object is active
            if ((Radius <= MaxRadius) && _active)
            {
                Radius += Time.deltaTime * ExpansionSpeed;
                _toroid.transform.localScale = new Vector3(Radius, Radius, Radius);
            }
            else
            {
                _active = false;
                _toroid.SetActive(false);
            }
        }

        public override void Exit()
        {
            // reset toroid values
            Radius = 1f;
            _toroid.transform.localScale = Vector3.one;
            _active = false;
        }

        private void OnAnimationEnded()
        {
            _toroid.SetActive(true);
            _active = true;
        }
    }
}

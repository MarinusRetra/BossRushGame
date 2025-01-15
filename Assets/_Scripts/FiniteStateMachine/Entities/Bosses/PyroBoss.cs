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

        [field: Space, SerializeField, Tooltip("The shockwave behavior that is easily modified in the inspector.")]
        private Shockwave _shockwave;

        /// <summary>
        /// Set a new target for the boss to follow.
        /// </summary>
        /// <param name="newTarget"></param>
        public void SetNewTarget(NetworkTransform newTarget)
        {
            _targetTransform = newTarget;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {

        }
#endif
    }
}
